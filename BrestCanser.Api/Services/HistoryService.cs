using BrestCanser.Api.Contracts.History;
using Microsoft.AspNetCore.Identity;

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

		return Result.Success<IEnumerable<HistoryResponse>>(response);
	}
}
