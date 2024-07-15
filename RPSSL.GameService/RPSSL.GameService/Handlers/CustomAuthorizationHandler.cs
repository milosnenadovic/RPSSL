using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace RPSSL.GameService.Handlers;

public class CustomAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
    {
        if (!context.User.Identity?.IsAuthenticated ?? false)
        {
            context.Fail();
            throw new UnauthorizedAccessException("Authentication token is not provided.");
        }

        if (!requirement.AllowedRoles.Any(role => context.User.IsInRole(role)))
        {
            context.Fail();
            throw new UnauthorizedAccessException("User does not have required role.");
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}