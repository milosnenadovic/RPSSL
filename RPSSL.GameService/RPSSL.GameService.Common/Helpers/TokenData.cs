using System.ComponentModel;

namespace RPSSL.GameService.Common.Helpers;

public enum TokenData
{
    [Description("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")]
    Email = 1,
    [Description("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")]
    Name = 2,
    [Description("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
    UserId = 3,
    [Description("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")]
    Role = 4,
    [Description("authtoken")]
    AuthToken = 5
}
