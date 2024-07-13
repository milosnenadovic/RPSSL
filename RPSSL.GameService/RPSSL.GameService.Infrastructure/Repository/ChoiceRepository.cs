﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Infrastructure.Persistence;

namespace RPSSL.GameService.Infrastructure.Repository;

public class ChoiceRepository : IChoiceRepository
{
	#region Setup
	private readonly IApplicationDbContext _dbContext;
	private readonly ILogger<ChoiceRepository> _logger;

	public ChoiceRepository(IApplicationDbContext dbContext, ILogger<ChoiceRepository> logger)
		=> (_dbContext, _logger) = (dbContext, logger);
	#endregion

	#region GetById
	public async Task<Choice?> GetById(int id, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Choice
			.AsNoTracking()
			.Include(x => x.ChoiceWins)
			.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
	}
	#endregion

	#region GetChoices
	public async Task<IEnumerable<Choice>> GetChoices(GetChoicesFilter filter, CancellationToken cancellationToken = default)
	{
		var choices = _dbContext.Choice
			.AsNoTracking()
			.Include(x => x.ChoiceWins)
			.AsQueryable();

		if (!string.IsNullOrEmpty(filter.FilterName))
			choices = choices.Where(x => x.Name.Contains(filter.FilterName, StringComparison.CurrentCultureIgnoreCase));

		if (filter.Active is not null)
			choices = choices.Where(x => x.Active == filter.Active);

		if (!await choices.AnyAsync(cancellationToken))
			return await Task.FromResult(Enumerable.Empty<Choice>());

		return await Task.FromResult(choices.AsEnumerable());
	}
	#endregion

	#region Add
	public async Task<bool> Add(Choice choice)
	{
		_dbContext.Choice.Add(choice);

		return await Task.FromResult(true);
	}
	#endregion

	#region Update
	public async Task<bool> Update(Choice choice, CancellationToken cancellationToken = default)
	{
		var dbChoice = await _dbContext.Choice.FindAsync([choice.Id], cancellationToken: cancellationToken);

		if (dbChoice is null)
			return await Task.FromResult(false);

		dbChoice.Name = choice.Name;
		dbChoice.Image = choice.Image;

		return await Task.FromResult(true);
	}
	#endregion
}