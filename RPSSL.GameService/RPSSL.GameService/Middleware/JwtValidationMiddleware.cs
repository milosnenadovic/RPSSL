using RPSSL.GameService.Common.Configurations;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Helpers.Auth;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Contracts._Common;
using System.Net;
using System.Text.Json;

namespace RPSSL.GameService.Middleware;

public class JwtValidationMiddleware(RequestDelegate next, Serilog.ILogger logger)
{
    private readonly RequestDelegate _next = next;
    private readonly Serilog.ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        var authToken = context.Request?.Headers[HttpContextItemKeys.Authorization];
        bool validToken = true;

        if (!string.IsNullOrEmpty(authToken))
        {
            validToken = await CheckAuthToken(authToken!);
        }
        if (!validToken)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var errorEpiResponse = new ErrorApiResponse()
            {
                Detail = Error.TokenParse.JwtMalformed,
                Title = ErrorCodes.TokenParse.ToString(),
                ErrorCode = (int)ErrorCodes.TokenParse,
                ErrorCodes = []
            };

            var result = JsonSerializer.Serialize(errorEpiResponse);
            await response.WriteAsync(result);
        }

        await _next(context);
    }

    private async Task<bool> CheckAuthToken(string authToken)
    {
        var userId = await AuthTokenHelper.GetUserId(authToken);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.Error("JWT validation failed: userId is empty.");
            return false;
        }

        var roleId = await AuthTokenHelper.GetRoleId(authToken);
        if (roleId is null)
        {
            _logger.Error("JWT validation failed: roleId is null.");
            return false;
        }

        var email = await AuthTokenHelper.GetEmail(authToken);
        if (email is null)
        {
            _logger.Error("JWT validation failed: email is null.");
            return false;
        }

        var username = await AuthTokenHelper.GetUsername(authToken);
        if (string.IsNullOrEmpty(username))
        {
            _logger.Error("JWT validation failed: username is empty.");
            return false;
        }

        return true;
    }
}