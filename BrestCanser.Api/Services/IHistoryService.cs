using BrestCanser.Api.Contracts.History;

namespace BrestCanser.Api.Services;

public interface IHistoryService
{
	Task<Result<IEnumerable<HistoryResponse>>> GetHistoryAsync(string userId);
	Task<Result<IEnumerable<HistoryResponse>>> GetHistoryWithStatusAsync(string userId, PredictionStatus? status = null);
	Task<Result<StatsResponse>> GetStatisticsAsync(string userId);
	Task<Result<ReportResponse>> GenerateReportAsync(string userId);
}
