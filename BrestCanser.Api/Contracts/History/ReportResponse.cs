namespace BrestCanser.Api.Contracts.History;

public record ReportResponse(
	byte[] FileContents,
	string ContentType,
	string FileName
);