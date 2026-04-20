namespace BrestCanser.Api.Errors;

public class MLErrors
{
	public static readonly Error InvalidPrediction =
		new("ML.Failed", "Prediction failed", StatusCodes.Status500InternalServerError);
}
