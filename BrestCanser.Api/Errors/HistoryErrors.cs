namespace BrestCanser.Api.Errors;

public class HistoryErrors
{
	public static readonly Error HistoryNotFound =
		new("History.NoPredictionsFound", "No history found for this user", StatusCodes.Status404NotFound);

	public static readonly Error NoHistoryForReport =
	new("History.NoHistoryForReport", "No prediction history found to generate report.", StatusCodes.Status404NotFound);
}
