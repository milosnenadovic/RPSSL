using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RPSSL.GameService.Application._Common.Behaviours;
using RPSSL.GameService.Application._Common.Mapping;
using RPSSL.GameService.Common;
using System.Reflection;

namespace RPSSL.GameService.Application._Common;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.RegisterMapsterConfiguration();

        services.AddCommonServices();

        #region Behaviours pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        #endregion

        return services;
    }
}
