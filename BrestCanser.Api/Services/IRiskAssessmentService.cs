using BrestCanser.Api.Contracts.RiskAssessment;

namespace BrestCanser.Api.Services;

public interface IRiskAssessmentService
{
    Task<Result<RiskAssessmentResponse>> AssessAsync(RiskAssessmentRequest request, string userId, CancellationToken ct = default);

    Task<Result<IEnumerable<RiskAssessmentHistory>>> GetHistoryAsync(string userId, CancellationToken ct = default);
}
