namespace BrestCanser.Api.Errors;

public class RiskAssessmentErrors
{

	public static readonly Error InvalidUserId =
		new("RiskAssessment.InvalidUserId",
			"User ID is missing or invalid.",
			StatusCodes.Status400BadRequest);

	public static Error EngineFailure(string detail) =>
		new("RiskAssessment.EngineFailure",
			$"Risk engine encountered an error: {detail}",
			StatusCodes.Status500InternalServerError);

	public static Error PersistenceFailed(string detail) =>
		new("RiskAssessment.PersistenceFailed",
			$"Failed to save assessment to database: {detail}",
			StatusCodes.Status500InternalServerError);

	public static Error DatabaseError(string detail) =>
		new("RiskAssessment.DatabaseError",
			$"Failed to retrieve assessment history: {detail}",
			StatusCodes.Status500InternalServerError);
}
