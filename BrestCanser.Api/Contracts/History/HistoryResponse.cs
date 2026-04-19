namespace BrestCanser.Api.Contracts.History;

public record HistoryResponse(
	 string ImageUrl,
	 string Diagnosis,
	 double Confidence,
	 string Status,
	 string Message,
	 DateTime CreatedAt
);
