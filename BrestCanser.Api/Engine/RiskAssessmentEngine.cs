using BrestCanser.Api.Contracts.RiskAssessment;
using BrestCanser.Api.Enum;
using BrestCanser.Api.Options;
using Microsoft.Extensions.Options;

namespace BrestCanser.Api.Engine;

public sealed class RiskAssessmentEngine
{
    private readonly Dictionary<string, int> _weights;
    private readonly int _maxScore;

    public RiskAssessmentEngine(IOptions<RiskScoringOptions> options)
    {
        var cfg = options.Value;
        _weights = cfg.Weights;
        _maxScore = cfg.MaxScore > 0 ? cfg.MaxScore : 135;
    }

    public RiskAssessmentResponse Evaluate(RiskAssessmentRequest r)
    {
        int score = 0;
        var reasons = new List<string>();

        void Add(string key, string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason)
                && _weights.TryGetValue(key, out int w)
                && w > 0)
            {
                score += w;
                reasons.Add(reason);
            }
        }

        //  Stage 1: Personal Info 
        Add($"AgeGroup.{r.AgeGroup}", r.AgeGroup switch
        {
            AgeGroup.Above50 => "age above 50",
            AgeGroup.From40To50 => "age between 40 and 50",
            AgeGroup.From30To39 => "age between 30 and 39",
            _ => string.Empty          // Under30
        });

        Add($"Ethnicity.{r.Ethnicity}", r.Ethnicity switch
        {
            Ethnicity.ArabCaucasian => "Arab/Caucasian ethnicity",
            Ethnicity.African => "African ethnicity",
            Ethnicity.Asian => "Asian ethnicity",
            _ => string.Empty      // Other
        });

        Add($"BmiCategory.{r.BmiCategory}", r.BmiCategory switch
        {
            BmiCategory.Obese => "obese BMI (increased estrogen production)",
            BmiCategory.Overweight => "overweight BMI",
            _ => string.Empty       // Normal
        });

        // Stage 2: Hormonal History
        Add($"MenarcheAge.{r.MenarcheAge}", r.MenarcheAge switch
        {
            MenarcheAge.Before12 => "early menarche before age 12 (prolonged estrogen exposure)",
            _ => string.Empty
        });

        Add($"PregnancyHistory.{r.PregnancyHistory}", r.PregnancyHistory switch
        {
            PregnancyHistory.NeverPregnant => "nulliparity (never pregnant)",
            PregnancyHistory.FirstChildAfter30 => "first pregnancy after age 30",
            _ => string.Empty  // FirstChildBefore30
        });

        Add($"MenopauseStatus.{r.MenopauseStatus}", r.MenopauseStatus switch
        {
            MenopauseStatus.YesWithHRT => "post-menopausal with hormone replacement therapy (HRT)",
            MenopauseStatus.YesWithoutHRT => "post-menopausal status",
            _ => string.Empty       // NotYet
        });

        // Stage 3: Family & Genetic History
        Add($"FamilyHistoryLevel.{r.FamilyHistoryLevel}", r.FamilyHistoryLevel switch
        {
            FamilyHistoryLevel.MoreThanOne => "multiple relatives with breast cancer",
            FamilyHistoryLevel.OneRelative => "one first-degree relative with breast cancer",
            _ => string.Empty      // None
        });

        Add($"EarlyFamilyDiagnosis.{r.EarlyFamilyDiagnosis}", r.EarlyFamilyDiagnosis switch
        {
            EarlyFamilyDiagnosis.Yes => "family member diagnosed before age 50",
            _ => string.Empty
        });

        Add($"BrcaMutation.{r.BrcaMutation}", r.BrcaMutation switch
        {
            BrcaMutation.Yes => "BRCA gene mutation confirmed",
            _ => string.Empty                    // NoOrNotTested
        });

        // Stage 4: Medical History
        Add($"BreastDensity.{r.BreastDensity}", r.BreastDensity switch
        {
            BreastDensity.Yes => "high breast density (independent risk factor, masks lesions)",
            _ => string.Empty                   // No / NeverHadImaging
        });

        Add($"BiopsyResult.{r.BiopsyResult}", r.BiopsyResult switch
        {
            BiopsyResult.Yes => "prior biopsy with abnormal or atypical findings",
            _ => string.Empty                    // NoOrBenign / NeverHadBiopsy
        });

        Add($"RadiationHistory.{r.RadiationHistory}", r.RadiationHistory switch
        {
            RadiationHistory.Yes => "prior chest wall radiation therapy",
            _ => string.Empty
        });

        // Interaction Rules
        if (r.BrcaMutation == BrcaMutation.Yes &&
            r.FamilyHistoryLevel == FamilyHistoryLevel.MoreThanOne)
        {
            score += 15;
            reasons.Add("compounded risk: BRCA mutation combined with multiple affected relatives");
        }

        if (r.EarlyFamilyDiagnosis == EarlyFamilyDiagnosis.Yes &&
            r.FamilyHistoryLevel != FamilyHistoryLevel.None)
        {
            score += 8;
            reasons.Add("early family diagnosis alongside existing family history");
        }

        if (r.MenopauseStatus == MenopauseStatus.YesWithHRT &&
            r.BmiCategory == BmiCategory.Obese)
        {
            score += 6;
            reasons.Add("combined effect of HRT and obesity on estrogen levels");
        }

        //Normalize
        double probability = Math.Round(score / (double)_maxScore * 100.0, 1);
        probability = Math.Min(probability, 100.0);

        //  Risk Level 
        string riskLevel = probability switch
        {
            < 30 => "Low",
            < 60 => "Moderate",
            _ => "High"
        };

        //  Classification 
        string classification = riskLevel == "High" ? "Malignant" : "Benign";

        //  Reasoning 
        string reasoning = reasons.Count == 0
            ? "No major high-risk factors identified."
            : $"Risk influenced by: {string.Join(", ", reasons)}.";

        return new RiskAssessmentResponse(riskLevel, probability, classification, reasoning);
    }
}
