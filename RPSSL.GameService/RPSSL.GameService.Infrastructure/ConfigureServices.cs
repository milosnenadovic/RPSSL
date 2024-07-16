using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Infrastructure.Persistence;
using RPSSL.GameService.Infrastructure.Persistence.Interceptors;
using RPSSL.GameService.Infrastructure.Repository;
using RPSSL.GameService.Infrastructure.Services.RandomGeneratedNumber;

namespace RPSSL.GameService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("RPSSL.GameService.Infrastructure")));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddHttpClient("RandomGeneratedNumberAPI", options =>
        {
            options.BaseAddress = new Uri(configuration.GetSection("Services:RandomGeneratedNumber:BaseUrl").Value!);
        });

        services.AddMemoryCache();

        services.AddScoped<IRandomGeneratedNumberService, RandomGeneratedNumberService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChoiceRepository, ChoiceRepository>();
        services.AddScoped<IChoiceWinRepository, ChoiceWinRepository>();
        services.AddScoped<ILocalizationRepository, LocalizationRepository>();
        services.AddScoped<IChoicesHistoryRepository, ChoicesHistoryRepository>();
        services.AddScoped<ILocalizationRepository, LocalizationRepository>();

        return services;
    }
}