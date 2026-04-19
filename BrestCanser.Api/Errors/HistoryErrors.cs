namespace BrestCanser.Api.Errors;

public class HistoryErrors
{
	public static readonly Error HistoryNotFound =
		new("History.NotFound", "No history found for this user", StatusCodes.Status404NotFound);
}
