using RPSSL.GameService.Infrastructure.Persistence;

namespace RPSSL.GameService.Extensions;

public static class InitializationExtensions
{
    public static async Task ApplyInitialization(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}
