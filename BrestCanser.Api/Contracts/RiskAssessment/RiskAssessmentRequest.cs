using BrestCanser.Api.Enum;

namespace BrestCanser.Api.Contracts.RiskAssessment;

public record RiskAssessmentRequest
(
    // Stage 1: Personal Info
    AgeGroup AgeGroup,
    Ethnicity Ethnicity,
    BmiCategory BmiCategory,

    // Stage 2: Hormonal History
    MenarcheAge MenarcheAge,
    PregnancyHistory PregnancyHistory,
    MenopauseStatus MenopauseStatus,

    // Stage 3: Family & Genetic History
    FamilyHistoryLevel FamilyHistoryLevel,
    EarlyFamilyDiagnosis EarlyFamilyDiagnosis,
    BrcaMutation BrcaMutation,

    // Stage 4: Medical History
    BreastDensity BreastDensity,
    BiopsyResult BiopsyResult,
    RadiationHistory RadiationHistory
);