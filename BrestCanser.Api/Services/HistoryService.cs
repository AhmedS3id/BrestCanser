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

	public async Task<Result<StatsResponse>> GetStatisticsAsync(string userId)
	{
		var histories = await _context.PredictionHistories
			.Where(x => x.UserId == userId)
			.ToListAsync();

		if (!histories.Any())
			return Result.Failure<StatsResponse>(HistoryErrors.HistoryNotFound);

		var total = histories.Count;

		var benignCount = histories.Count(x => x.Status == PredictionStatus.Benign);
		var malignantCount = histories.Count(x => x.Status == PredictionStatus.Malignant);
		var uncertainCount = histories.Count(x => x.Status == PredictionStatus.Uncertain);

		var response = new StatsResponse(
			 total,
			 benignCount,
			 malignantCount,
			 uncertainCount,
			 Math.Round((double)benignCount / total * 100, 2),
			 Math.Round((double)malignantCount / total * 100, 2),
			 Math.Round((double)uncertainCount / total * 100, 2),
			 Math.Round(histories.Average(x => x.Confidence), 2),
			 DateOnly.FromDateTime(histories.Max(x => x.CreatedAt))
		);

		return Result.Success(response);
	}
}
