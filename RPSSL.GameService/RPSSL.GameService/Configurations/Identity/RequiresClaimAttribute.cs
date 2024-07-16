using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Configurations.Identity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresClaimAttribute(string requiredClaimName, string requiredClaimValue) : Attribute, IAuthorizationFilter
{
    private readonly string _requiredClaimName = requiredClaimName;
    private readonly string _requiredClaimValue = requiredClaimValue;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.HasClaim(_requiredClaimName, _requiredClaimValue))
        {
            context.Result = new ForbidResult(Error.Authorization.MissingClaim + _requiredClaimName);
        }
    }
}
