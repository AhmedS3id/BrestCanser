using BrestCanser.Api.Contracts.RiskAssessment;

public interface IRiskAssessmentService
{
	Task<Result<RiskAssessmentResponse>> AssessAsync(
		RiskAssessmentRequest request,
		CancellationToken ct = default);
}