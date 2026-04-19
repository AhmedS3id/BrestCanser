using BrestCanser.Api.Contracts.History;
using BrestCanser.Api.Enum;

namespace BrestCanser.Api.Services;

public interface IHistoryService
{
	Task<Result<IEnumerable<HistoryResponse>>> GetHistoryAsync(string userId);
	Task<Result<IEnumerable<HistoryResponse>>> GetHistoryWithStatusAsync(string userId, PredictionStatus? status = null);
}
