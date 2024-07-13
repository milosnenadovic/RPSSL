﻿using RPSSL.GameService.Domain.Abstractions;

namespace RPSSL.GameService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
	private readonly IApplicationDbContext _context;

	public UnitOfWork(IApplicationDbContext context) => _context = context;

	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return _context.SaveChangesAsync(cancellationToken);
	}
}