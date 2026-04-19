using BrestCanser.Api.Contracts.History;
using BrestCanser.Api.Enum;

namespace BrestCanser.Api.Services;

public class HistoryService : IHistoryService
{
	private readonly ApplicationDbContext _context;
	public HistoryService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<IEnumerable<HistoryResponse>>> GetHistoryAsync(string userId)
	{
		var histories = await _context.PredictionHistories
			.Where(x => x.UserId == userId)
			.OrderByDescending(x => x.CreatedAt)
			.ToListAsync();


		if (!histories.Any())
			return Result.Failure<IEnumerable<HistoryResponse>>(HistoryErrors.HistoryNotFound);

		var response = histories.Adapt<IEnumerable<HistoryResponse>>();

		return Result.Success(response);
	}
	public async Task<Result<IEnumerable<HistoryResponse>>> GetHistoryWithStatusAsync(string userId, PredictionStatus? status = null)
	{
		var query = _context.PredictionHistories
			.Where(x => x.UserId == userId);

		if (status.HasValue)
		{
			query = query.Where(x => x.Status == status.Value);
		}

		var histories = await query
			.OrderByDescending(x => x.CreatedAt)
			.ToListAsync();

		if (!histories.Any())
			return Result.Failure<IEnumerable<HistoryResponse>>(HistoryErrors.HistoryNotFound);

		var response = histories.Adapt<IEnumerable<HistoryResponse>>();

		return Result.Success(response);
	}
}
