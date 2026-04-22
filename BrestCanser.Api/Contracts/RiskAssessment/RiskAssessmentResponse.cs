namespace BrestCanser.Api.Contracts.RiskAssessment;

public record RiskAssessmentResponse
(
    string RiskLevel,          // Low / Moderate / High
    double RiskProbability,    // e.g. 72.5
    string Classification,     // Benign / Malignant
    string Reasoning           // Medical-style explanation
);
