using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Infrastructure.Persistence.Interceptors;
using RPSSL.GameService.Infrastructure.Settings;
using System.Reflection;

namespace RPSSL.GameService.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor, IOptions<DatabaseSettings> databaseSettings) : IdentityDbContext<User>(options), IApplicationDbContext
{
	public class OptionsBuild
	{
		public OptionsBuild(DatabaseSettings databaseSettings)
		{
			OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			OptionsBuilder.UseNpgsql(databaseSettings.DefaultConnection);
			OptionsBuilder.EnableSensitiveDataLogging();
			DatabaseOptions = OptionsBuilder.Options;
		}

		public DbContextOptionsBuilder<ApplicationDbContext> OptionsBuilder { get; set; }
		public DbContextOptions<ApplicationDbContext> DatabaseOptions { get; set; }
	}

	public readonly OptionsBuild options = new(databaseSettings.Value);

	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

	public DbSet<User> User { get; set; }
	public DbSet<Choice> Choice { get; set; }
	public DbSet<ChoiceWin> ChoiceWin { get; set; }
	public DbSet<ChoicesHistory> ChoicesHistory { get; set; }
	public DbSet<Language> Language { get; set; }
	public DbSet<LocalizationLabel> LocalizationLabel { get; set; }

	public override DatabaseFacade Database => base.Database;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		foreach (var entityType in builder.Model.GetEntityTypes())
			foreach (var property in entityType.GetProperties())
				if (property.ClrType == typeof(DateTime))
					property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
						v => v.ToUniversalTime(),
						v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(builder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<string>(x => x.HaveMaxLength(96));
		configurationBuilder.Properties<Enum>(x => x.HaveMaxLength(24));
		configurationBuilder.Properties<decimal>().HavePrecision(16, 4);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var result = await base.SaveChangesAsync(cancellationToken);
		return result;
	}

	public async Task<int> SaveChangesAsync()
	{
		var result = await SaveChangesAsync(CancellationToken.None);
		return result;
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		var result = base.SaveChanges(acceptAllChangesOnSuccess);
		return result;
	}

	public override int SaveChanges()
	{
		var result = SaveChanges(false);
		return result;
	}
}
