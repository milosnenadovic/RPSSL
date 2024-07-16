namespace RPSSL.Web.Configurations;

public record AuthTokenClaims
{
    public const string Id = "nameidentifier";
    public const string Name = "name";
    public const string Email = "emailaddress";
    public const string Role = "role";
    public const string OrganizationId = "organizationid";
    public const string UserOrganizationId = "userorganizationid";
}
