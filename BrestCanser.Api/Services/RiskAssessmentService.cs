using BrestCanser.Api.Contracts.RiskAssessment;
using BrestCanser.Api.Engine;
using System.Text.Json;

namespace BrestCanser.Api.Services;

public class RiskAssessmentService : IRiskAssessmentService
{
    
    private readonly RiskAssessmentEngine _engine;
    private readonly ApplicationDbContext _db;

    public RiskAssessmentService(RiskAssessmentEngine engine, ApplicationDbContext db)
    {
        _engine = engine;
        _db = db;
    }

    public async Task<Result<RiskAssessmentResponse>> AssessAsync(
        RiskAssessmentRequest request,
        string userId,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Failure<RiskAssessmentResponse>(
                RiskAssessmentErrors.InvalidUserId);

        RiskAssessmentResponse response;

        try
        {
            response = _engine.Evaluate(request);
        }
        catch (Exception ex)
        {
            return Result.Failure<RiskAssessmentResponse>(
                RiskAssessmentErrors.EngineFailure(ex.Message));
        }

        var history = new RiskAssessmentHistory
        {
            UserId = userId,
            AgeGroup = request.AgeGroup,
            Ethnicity = request.Ethnicity,
            BmiCategory = request.BmiCategory,
            MenarcheAge = request.MenarcheAge,
            PregnancyHistory = request.PregnancyHistory,
            MenopauseStatus = request.MenopauseStatus,
            FamilyHistoryLevel = request.FamilyHistoryLevel,
            EarlyFamilyDiagnosis = request.EarlyFamilyDiagnosis,
            BrcaMutation = request.BrcaMutation,
            BreastDensity = request.BreastDensity,
            BiopsyResult = request.BiopsyResult,
            RadiationHistory = request.RadiationHistory,
            RiskLevel = response.RiskLevel,
            RiskProbability = response.RiskProbability,
            Classification = response.Classification,
            Reasoning = response.Reasoning,
            // Breakdown ↓
            BreakdownFamilyHistory = response.Breakdown.FamilyHistory,
            BreakdownLifestyle = response.Breakdown.Lifestyle,
            BreakdownGeneticFactors = response.Breakdown.GeneticFactors,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            _db.RiskAssessmentHistories.Add(history);
            await _db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            return Result.Failure<RiskAssessmentResponse>(
                RiskAssessmentErrors.PersistenceFailed(ex.Message));
        }

        return Result.Success(response);
    }


    public async Task<Result<IEnumerable<RiskAssessmentHistory>>> GetHistoryAsync(
    string userId,
    CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Failure<IEnumerable<RiskAssessmentHistory>>(
                RiskAssessmentErrors.InvalidUserId);

        try
        {
            var history = await _db.RiskAssessmentHistories
                .AsNoTracking()
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync(ct);

            return Result.Success<IEnumerable<RiskAssessmentHistory>>(history);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<RiskAssessmentHistory>>(
                RiskAssessmentErrors.DatabaseError(ex.Message));
        }
    }
}
