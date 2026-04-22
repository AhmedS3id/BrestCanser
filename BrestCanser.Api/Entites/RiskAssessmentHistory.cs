using BrestCanser.Api.Enum;

namespace BrestCanser.Api.Entites;

public class RiskAssessmentHistory
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Input fields
    public AgeGroup AgeGroup { get; set; }
    public Ethnicity Ethnicity { get; set; }
    public BmiCategory BmiCategory { get; set; }
    public MenarcheAge MenarcheAge { get; set; }
    public PregnancyHistory PregnancyHistory { get; set; }
    public MenopauseStatus MenopauseStatus { get; set; }
    public FamilyHistoryLevel FamilyHistoryLevel { get; set; }
    public EarlyFamilyDiagnosis EarlyFamilyDiagnosis { get; set; }
    public BrcaMutation BrcaMutation { get; set; }
    public BreastDensity BreastDensity { get; set; }
    public BiopsyResult BiopsyResult { get; set; }
    public RadiationHistory RadiationHistory { get; set; }

    // Output
    public string RiskLevel { get; set; } = string.Empty;
    public double RiskProbability { get; set; }
    public string Classification { get; set; } = string.Empty;
    public string Reasoning { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
