using Microsoft.EntityFrameworkCore;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Enums;
using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Infrastructure.Persistence;

namespace RPSSL.GameService.Infrastructure.Repository;

public class ChoicesHistoryRepository : IChoicesHistoryRepository
{
	#region Setup
	private readonly IApplicationDbContext _dbContext;

	public ChoicesHistoryRepository(IApplicationDbContext dbContext) => _dbContext = dbContext;
	#endregion

	#region GetChoicesHistory
	/// <summary>
	/// Get paginated choices history by playerId, playerChoiceId, computerChoiceId, result, createdFrom, createdTo or active properties
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public async Task<KeyValuePair<IEnumerable<ChoicesHistory>, int>> GetChoicesHistory(GetChoicesHistoryFilter filter, CancellationToken cancellationToken = default)
	{
		var choicesHistory = _dbContext.ChoicesHistory
			.AsNoTracking()
			.Where(x => x.PlayerId == filter.PlayerId);

		if (filter.PlayerChoiceId is not null)
			choicesHistory = choicesHistory.Where(x => x.PlayerChoiceId == filter.PlayerChoiceId);

		if (filter.ComputerChoiceId is not null)
			choicesHistory = choicesHistory.Where(x => x.ComputerChoiceId == filter.ComputerChoiceId);

		if (filter.Result is not null)
			choicesHistory = choicesHistory.Where(x => FilterResult(x.PlayerChoiceId, x.ComputerChoiceId, (GameResult)filter.Result));

		if (filter.Active is not null)
			choicesHistory = choicesHistory.Where(x => x.Active == filter.Active);

		if (filter.CreatedFrom is not null)
			choicesHistory = choicesHistory.Where(x => x.Created >= filter.CreatedFrom);

		if (filter.CreatedTo is not null)
			choicesHistory = choicesHistory.Where(x => x.Created <= filter.CreatedTo);

		switch (filter.SortBy)
		{
			case 1:
				if (filter.SortDescending)
					choicesHistory = choicesHistory.OrderByDescending(x => x.PlayerChoiceId);
				else
					choicesHistory = choicesHistory.OrderBy(x => x.PlayerChoiceId);
				break;
			case 2:
				if (filter.SortDescending)
					choicesHistory = choicesHistory.OrderByDescending(x => x.ComputerChoiceId);
				else
					choicesHistory = choicesHistory.OrderBy(x => x.ComputerChoiceId);
				break;
			case 3:
				if (filter.SortDescending)
					choicesHistory = choicesHistory.OrderByDescending(x => x.Created);
				else
					choicesHistory = choicesHistory.OrderBy(x => x.Created);
				break;
			case 4:
				if (filter.SortDescending)
					choicesHistory = choicesHistory.OrderByDescending(x => x.Active);
				else
					choicesHistory = choicesHistory.OrderBy(x => x.Active);
				break;
			default:
				if (filter.SortDescending)
					choicesHistory = choicesHistory.OrderByDescending(x => x.Id);
				else
					choicesHistory = choicesHistory.OrderBy(x => x.Id);
				break;
		}

		if (!await choicesHistory.AnyAsync(cancellationToken))
			return await Task.FromResult(new KeyValuePair<IEnumerable<ChoicesHistory>, int>([], 0));

		var count = await choicesHistory.CountAsync(cancellationToken);

		if (filter.PageNumber > 0)
			choicesHistory = choicesHistory.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

		return await Task.FromResult(new KeyValuePair<IEnumerable<ChoicesHistory>, int>(choicesHistory.AsEnumerable(), count));
	}
	#endregion

	#region Add
	/// <summary>
	/// Add new record to ChoicesHistory
	/// </summary>
	/// <param name="choicesHistory"></param>
	/// <returns></returns>
	public void Add(ChoicesHistory choicesHistory, CancellationToken cancellationToken = default)
	{
		_dbContext.ChoicesHistory.Add(choicesHistory);
	}
	#endregion

	#region DeleteAll
	/// <summary>
	/// Soft delete all records for a user
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public async Task DeleteAll(string userId, CancellationToken cancellationToken = default)
	{
		var rowsToUpdate = await _dbContext.ChoicesHistory
			.Where(x => x.PlayerId == userId && x.Active)
			.ToListAsync();

		foreach (var row in rowsToUpdate)
			row.Active = false;
	}
	#endregion

	#region Private helper methods
	/// <summary>
	/// Calculates result of the game based on wanted result
	/// </summary>
	/// <param name="playerChoiceId"></param>
	/// <param name="computerChoiceId"></param>
	/// <param name="result"></param>
	/// <returns></returns>
	private bool FilterResult(int playerChoiceId, int computerChoiceId, GameResult result)
	{
		var wins = _dbContext.ChoiceWin
			.AsNoTracking()
			.Where(x => x.ChoiceId == playerChoiceId && x.Active)
			.Select(x => x.BeatsChoiceId);

		return result switch
		{
			GameResult.Tie => playerChoiceId == computerChoiceId,
			GameResult.Win => wins.Any(x => x == computerChoiceId),
			GameResult.Lose => !wins.Any(x => x == computerChoiceId),
			_ => false,
		};
	}
	#endregion
}
