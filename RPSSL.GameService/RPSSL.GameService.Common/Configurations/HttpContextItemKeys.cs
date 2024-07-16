namespace RPSSL.GameService.Common.Configurations;

public record HttpContextItemKeys
{
    public const string AppSecret = "AppSecret";
    public const string Authorization = "Authorization";
    public const string OAuth2 = "oauth2";
    public const string Bearer = "Bearer";

    public const string AuthToken = "authToken";
    public const string RefreshToken = "refreshToken";

    public const string AuthTokenCookie = "X-Auth-Token";
    public const string RefreshTokenCookie = "X-Refresh-Token";
}
