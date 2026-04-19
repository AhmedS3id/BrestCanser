namespace BrestCanser.Api.Contracts.History;

public record HistoryResponse(
	 string ImageUrl,
	 string Diagnosis,
	 double Confidence,
	 DateOnly CreatedAt
);
