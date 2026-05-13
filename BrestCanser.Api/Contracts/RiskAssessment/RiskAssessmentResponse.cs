namespace BrestCanser.Api.Contracts.RiskAssessment;

public record RiskAssessmentResponse(
	string RiskLevel,            // Low / Moderate / High
	double RiskProbability,      // e.g. 72.5
	string Classification,       // Benign / Malignant
	CategoryBreakdown Breakdown
);

public record CategoryBreakdown(
	string FamilyHistory,    // Low / Moderate / High
	string Lifestyle,        // Low / Moderate / High
	string GeneticFactors    // Low / Moderate / High
);