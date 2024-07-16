namespace RPSSL.GameService.Common.Constants.Errors;

public static partial class Error
{
    public static class Authorization
    {
        public static string User => "ErrorAuthorizationUser";
        public static string UserInactive => "ErrorAuthorizationUserInactive";
        public static string UnconfirmedEmail => "ErrorAuthorizationUnconfirmedEmail";
        public static string OrganizationUser => "ErrorAuthorizationOrganizationUser";
        public static string Role => "ErrorAuthorizationRole";
        public static string CantResolveUser => "ErrorAuthorizationCantResolveUser";
        public static string Login => "ErrorAuthorizationLogin";
        public static string MissingClaim => "ErrorAuthorizationMissingClaim";
        public static string MissingToken => "ErrorAuthorizationMissingToken";
        public static string TokenExpired => "ErrorAuthorizationTokenExpired";
    }
}
