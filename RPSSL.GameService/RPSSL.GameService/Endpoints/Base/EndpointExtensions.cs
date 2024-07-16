using System.Reflection;

namespace RPSSL.GameService.Endpoints.Base;

public static class EndpointExtensions
{
    public static void AddEndpoints<TMarker>(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(typeof(TMarker), configuration);
    }

    private static void AddEndpoints(this IServiceCollection services, Type typeMarker, IConfiguration configuration)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);

        foreach (var endpointType in endpointTypes)
            endpointType.GetMethod(nameof(IEndpoints.AddServices))!
                .Invoke(null, [services, configuration]);
    }

    public static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        app.UseEndpoints(typeof(TMarker));
    }

    private static void UseEndpoints(this IApplicationBuilder app, Type typeMarker)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);

        foreach (var endpointType in endpointTypes)
            endpointType.GetMethod(nameof(IEndpoints.DefineEndpoints))!
                .Invoke(null, [app]);
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
    {
        var endpointTypes = typeMarker
            .Assembly
            .DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface && typeof(IEndpoints).IsAssignableFrom(x));

        return endpointTypes;
    }
}
