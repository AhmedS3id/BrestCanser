namespace BrestCanser.Api.Contracts.History;

public record StatsResponse(
	int TotalPredictions,
	int BenignCount,
	int MalignantCount,
	int UncertainCount,
	double BenignPercentage,
	double MalignantPercentage,
	double UncertainPercentage,
	double AverageConfidence,
	DateOnly LastScanDate
);