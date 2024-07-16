namespace RPSSL.GameService.Endpoints.Base;

public interface IEndpoints
{
    public static abstract void DefineEndpoints(IEndpointRouteBuilder builder);
    public static abstract void AddServices(IServiceCollection services, IConfiguration configuration);
}
