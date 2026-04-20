namespace BrestCanser.Api.Errors;

public class HistoryErrors
{
	public static readonly Error HistoryNotFound =
		new("History.NoPredictionsFound", "No history found for this user", StatusCodes.Status404NotFound);
}
