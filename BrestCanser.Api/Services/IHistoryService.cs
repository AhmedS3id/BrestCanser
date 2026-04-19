using BrestCanser.Api.Contracts.History;

namespace BrestCanser.Api.Services;

public interface IHistoryService
{
	Task<Result<IEnumerable<HistoryResponse>>> GetHistoryAsync(string userId);
}
