using Microsoft.EntityFrameworkCore;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Infrastructure.Persistence;

namespace RPSSL.GameService.Infrastructure.Repository;

public class ChoiceWinRepository : IChoiceWinRepository
{
	#region Setup
	private readonly IApplicationDbContext _dbContext;

	public ChoiceWinRepository(IApplicationDbContext dbContext)
		=> _dbContext = dbContext;
	#endregion

	#region GetChoiceWins
	/// <summary>
	/// Get active choice wins by choiceId properties
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public async Task<IEnumerable<ChoiceWin>> GetChoiceWins(int? choiceId, CancellationToken cancellationToken = default)
	{
		var choiceWins = _dbContext.ChoiceWin
			.AsNoTracking()
			.Where(x => x.Active);

		if (choiceId is not null)
			choiceWins = choiceWins.Where(x => x.ChoiceId == choiceId);

		if (!await choiceWins.AnyAsync(cancellationToken))
			return await Task.FromResult(Enumerable.Empty<ChoiceWin>());

		return await Task.FromResult(choiceWins.AsEnumerable());
	}
	#endregion
}
