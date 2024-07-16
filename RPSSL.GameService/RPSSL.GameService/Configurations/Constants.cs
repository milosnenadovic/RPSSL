namespace RPSSL.GameService.Configurations;

public static class Constants
{
    public const string CorsPolicy = "CORSPolicy";

    public const string SwaggerVersion = "v1";
    public const string SwaggerTitle = "RPSSL.GameService";
    public const string SwaggerDescription = "An ASP.NET 8 Web API for managing RPSSL game.";
    public const string SwaggerTerms = "https://tempuri.com/terms";
    public const string SwaggerEndpoint = "/swagger/v1/swagger.json";
    public const string SwaggerStyles = "/swagger-ui/custom.css";

    public const string OpenApiContactName = "RPSSL";
    public const string OpenApiContactUri = "https://tempuri.com/contact";
    public const string OpenApiLicenseName = "RPSSL license";
    public const string OpenApiLicenseUri = "https://tempuri.com/license";

    public const string ExceptionHandlerUri = "/Error";
    public const string FallbackFile = "index.razor";

    public const string HealthCheckUri = "/Health";
}
