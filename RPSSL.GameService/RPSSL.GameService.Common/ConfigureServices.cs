using Microsoft.Extensions.DependencyInjection;
using RPSSL.GameService.Common.Services;

namespace RPSSL.GameService.Common;

public static class ConfigureServices
{
	public static IServiceCollection AddCommonServices(this IServiceCollection services)
	{
		services.AddScoped<ICurrentUserService, CurrentUserService>();

		return services;
	}
}
