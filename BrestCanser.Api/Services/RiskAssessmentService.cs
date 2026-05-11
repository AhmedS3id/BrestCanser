using BrestCanser.Api.Contracts.RiskAssessment;
using BrestCanser.Api.Engine;

namespace BrestCanser.Api.Services;

public class RiskAssessmentService : IRiskAssessmentService
{
    private readonly RiskAssessmentEngine _engine;

    public RiskAssessmentService(RiskAssessmentEngine engine)
    {
        _engine = engine;
    }

    public async Task<Result<RiskAssessmentResponse>> AssessAsync(
        RiskAssessmentRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var response = _engine.Evaluate(request);

            return await Task.FromResult(Result.Success(response));
        }
        catch (Exception ex)
        {
            return Result.Failure<RiskAssessmentResponse>(
                RiskAssessmentErrors.EngineFailure(ex.Message));
        }
    }
}