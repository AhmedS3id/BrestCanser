using BrestCanser.Api.Contracts.RiskAssessment;
using BrestCanser.Api.Enum;
using BrestCanser.Api.Options;
using Microsoft.Extensions.Options;

namespace BrestCanser.Api.Engine;

public sealed class RiskAssessmentEngine
{
    private readonly Dictionary<string, int> _weights;
    private readonly int _maxScore;

    // Max scores per category (for normalization)
    private const int MaxFamilyScore = 48;  // FamilyHistoryLevel + EarlyFamilyDiagnosis + interaction
    private const int MaxLifestyleScore = 37;  // Age + BMI + Menarche + Pregnancy + Menopause + Radiation + HRT interaction
    private const int MaxGeneticScore = 50;  // BRCA + Ethnicity + BreastDensity + Biopsy + BRCA interaction

    public RiskAssessmentEngine(IOptions<RiskScoringOptions> options)
    {
        var cfg = options.Value;
        _weights = cfg.Weights;
        _maxScore = cfg.MaxScore > 0 ? cfg.MaxScore : 135;
    }

    public RiskAssessmentResponse Evaluate(RiskAssessmentRequest r)
    {
        int totalScore = 0;
        int familyScore = 0;
        int lifestyleScore = 0;
        int geneticScore = 0;
        var reasons = new List<string>();

        // ── Helpers ──────────────────────────────────────────────────────────
        void AddTo(ref int category, string key, string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason)
                && _weights.TryGetValue(key, out int w)
                && w > 0)
            {
                category += w;
                totalScore += w;
                reasons.Add(reason);
            }
        }

        // ── Stage 1: Personal Info → Lifestyle ───────────────────────────────
        AddTo(ref lifestyleScore, $"AgeGroup.{r.AgeGroup}", r.AgeGroup switch
        {
            AgeGroup.Above50 => "age above 50",
            AgeGroup.From40To50 => "age between 40 and 50",
            AgeGroup.From30To39 => "age between 30 and 39",
            _ => string.Empty
        });

        AddTo(ref lifestyleScore, $"BmiCategory.{r.BmiCategory}", r.BmiCategory switch
        {
            BmiCategory.Obese => "obese BMI (increased estrogen production)",
            BmiCategory.Overweight => "overweight BMI",
            _ => string.Empty
        });

        // ── Stage 2: Hormonal History → Lifestyle ────────────────────────────
        AddTo(ref lifestyleScore, $"MenarcheAge.{r.MenarcheAge}", r.MenarcheAge switch
        {
            MenarcheAge.Before12 => "early menarche before age 12 (prolonged estrogen exposure)",
            _ => string.Empty
        });

        AddTo(ref lifestyleScore, $"PregnancyHistory.{r.PregnancyHistory}", r.PregnancyHistory switch
        {
            PregnancyHistory.NeverPregnant => "nulliparity (never pregnant)",
            PregnancyHistory.FirstChildAfter30 => "first pregnancy after age 30",
            _ => string.Empty
        });

        AddTo(ref lifestyleScore, $"MenopauseStatus.{r.MenopauseStatus}", r.MenopauseStatus switch
        {
            MenopauseStatus.YesWithHRT => "post-menopausal with hormone replacement therapy",
            MenopauseStatus.YesWithoutHRT => "post-menopausal status",
            _ => string.Empty
        });

        AddTo(ref lifestyleScore, $"RadiationHistory.{r.RadiationHistory}", r.RadiationHistory switch
        {
            RadiationHistory.Yes => "prior chest wall radiation therapy",
            _ => string.Empty
        });

        // ── Stage 3: Family History → Family ─────────────────────────────────
        AddTo(ref familyScore, $"FamilyHistoryLevel.{r.FamilyHistoryLevel}", r.FamilyHistoryLevel switch
        {
            FamilyHistoryLevel.MoreThanOne => "multiple relatives with breast cancer",
            FamilyHistoryLevel.OneRelative => "one first-degree relative with breast cancer",
            _ => string.Empty
        });

        AddTo(ref familyScore, $"EarlyFamilyDiagnosis.{r.EarlyFamilyDiagnosis}", r.EarlyFamilyDiagnosis switch
        {
            EarlyFamilyDiagnosis.Yes => "family member diagnosed before age 50",
            _ => string.Empty
        });

        // ── Stage 4: Genetic / Medical → Genetic ─────────────────────────────
        AddTo(ref geneticScore, $"Ethnicity.{r.Ethnicity}", r.Ethnicity switch
        {
            Ethnicity.ArabCaucasian => "Arab/Caucasian ethnicity",
            Ethnicity.African => "African ethnicity",
            Ethnicity.Asian => "Asian ethnicity",
            _ => string.Empty
        });

        AddTo(ref geneticScore, $"BrcaMutation.{r.BrcaMutation}", r.BrcaMutation switch
        {
            BrcaMutation.Yes => "BRCA gene mutation confirmed",
            _ => string.Empty
        });

        AddTo(ref geneticScore, $"BreastDensity.{r.BreastDensity}", r.BreastDensity switch
        {
            BreastDensity.Yes => "high breast density (masks lesions, independent risk factor)",
            _ => string.Empty
        });

        AddTo(ref geneticScore, $"BiopsyResult.{r.BiopsyResult}", r.BiopsyResult switch
        {
            BiopsyResult.Yes => "prior biopsy with abnormal or atypical findings",
            _ => string.Empty
        });

        // ── Interaction Rules ─────────────────────────────────────────────────
        if (r.BrcaMutation == BrcaMutation.Yes &&
            r.FamilyHistoryLevel == FamilyHistoryLevel.MoreThanOne)
        {
            const int bonus = 15;
            geneticScore += bonus;
            familyScore += bonus;
            totalScore += bonus;
            reasons.Add("compounded risk: BRCA mutation combined with multiple affected relatives");
        }

        if (r.EarlyFamilyDiagnosis == EarlyFamilyDiagnosis.Yes &&
            r.FamilyHistoryLevel != FamilyHistoryLevel.None)
        {
            const int bonus = 8;
            familyScore += bonus;
            totalScore += bonus;
            reasons.Add("early family diagnosis alongside existing family history");
        }

        if (r.MenopauseStatus == MenopauseStatus.YesWithHRT &&
            r.BmiCategory == BmiCategory.Obese)
        {
            const int bonus = 6;
            lifestyleScore += bonus;
            totalScore += bonus;
            reasons.Add("combined effect of HRT and obesity on estrogen levels");
        }

        // ── Normalize total ───────────────────────────────────────────────────
        double probability = Math.Round(totalScore / (double)_maxScore * 100.0, 1);
        probability = Math.Min(probability, 100.0);

        // ── Category labels ───────────────────────────────────────────────────
        static string CategoryLabel(int catScore, int catMax) =>
            Math.Round(catScore / (double)catMax * 100.0, 1) switch
            {
                < 30 => "Low",
                < 60 => "Moderate",
                _ => "High"
            };

        var breakdown = new CategoryBreakdown(
            FamilyHistory: CategoryLabel(familyScore, MaxFamilyScore),
            Lifestyle: CategoryLabel(lifestyleScore, MaxLifestyleScore),
            GeneticFactors: CategoryLabel(geneticScore, MaxGeneticScore)
        );

        // ── Risk Level ────────────────────────────────────────────────────────
        string riskLevel = probability switch
        {
            < 30 => "Low",
            < 60 => "Moderate",
            _ => "High"
        };

        string classification = riskLevel == "High" ? "Malignant" : "Benign";

        string reasoning = reasons.Count == 0
            ? "No major high-risk factors identified."
            : $"Risk influenced by: {string.Join(", ", reasons)}.";

        return new RiskAssessmentResponse(riskLevel, probability, classification, reasoning, breakdown);
    }
}
