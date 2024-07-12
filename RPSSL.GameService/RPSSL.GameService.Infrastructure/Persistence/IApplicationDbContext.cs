using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence;

public interface IApplicationDbContext
{
	DbSet<User> User { get; set; }
	DbSet<Choice> Choice { get; set; }
	DbSet<ChoiceWin> ChoiceWin { get; set; }
	DbSet<ChoicesHistory> ChoicesHistory { get; set; }
	DbSet<Language> Language { get; set; }
	DbSet<LocalizationLabel> LocalizationLabel { get; set; }

	DatabaseFacade Database { get; }

	Task<int> SaveChangesAsync();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	int SaveChanges();
	int SaveChanges(bool acceptAllChangesOnSuccess);
}