namespace RPSSL.GameService.Common.Constants.Errors;

public static partial class Error
{
    public static class TokenParse
    {
        public static string JwtMalformed => "ErrorTokenParseJwtMalformed";
        public static string UserId => "ErrorTokenParseUserId";
        public static string Role => "ErrorTokenParseRole";
        public static string Email => "ErrorTokenParseEmail";
    }
}
