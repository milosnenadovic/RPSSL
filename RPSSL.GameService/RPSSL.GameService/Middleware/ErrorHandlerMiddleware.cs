using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Exceptions;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Contracts._Common;
using System.Net;
using System.Text.Json;

namespace RPSSL.GameService.Middleware;

public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException error)
        {
            context.Response.ContentType = "application/json";

            var errorApiResponse = new ErrorApiResponse
            {
                Detail = error.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            errorApiResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
            errorApiResponse.Title = Error.Validation.Middleware;
            errorApiResponse.ErrorCodes = [];
            foreach (var err in error.Errors)
            {
                foreach (string errorValue in err.Value)
                {
                    errorApiResponse.ErrorCodes.Add(new() { Code = err.Key, Description = errorValue });
                    errorApiResponse.Detail = errorValue;
                }
            }

            _logger.LogError(error, error.Message);

            var result = JsonSerializer.Serialize(errorApiResponse);
            await context.Response.WriteAsync(result);
        }
        catch (InvalidOperationException error) when (error.Message.Contains("The AuthorizationPolicy named"))
        {
            context.Response.ContentType = "application/json";

            var errorApiResponse = new ErrorApiResponse
            {
                Detail = error.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            errorApiResponse.ErrorCode = (int)HttpStatusCode.Unauthorized;
            errorApiResponse.Title = Error.Authorization.CantResolveUser;

            _logger.LogError(error, error.Message);

            var result = JsonSerializer.Serialize(errorApiResponse);
            await context.Response.WriteAsync(result);
        }
        catch (CantResolveUserException error)
        {
            context.Response.ContentType = "application/json";

            var errorApiResponse = new ErrorApiResponse
            {
                Detail = error.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            errorApiResponse.ErrorCode = (int)HttpStatusCode.Unauthorized;
            errorApiResponse.Title = Error.Authorization.CantResolveUser;

            _logger.LogError(error, error.Message);

            var result = JsonSerializer.Serialize(errorApiResponse);
            await context.Response.WriteAsync(result);
        }
        catch (UnauthorizedAccessException error)
        {
            context.Response.ContentType = "application/json";

            var errorApiResponse = new ErrorApiResponse
            {
                Detail = error.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            errorApiResponse.ErrorCode = (int)HttpStatusCode.Unauthorized;
            errorApiResponse.Title = Error.Authorization.MissingToken;

            _logger.LogError(error, error.Message);

            var result = JsonSerializer.Serialize(errorApiResponse);
            await context.Response.WriteAsync(result);
        }
        catch (KeyNotFoundException error)
        {
            context.Response.ContentType = "application/json";

            var errorApiResponse = new ErrorApiResponse
            {
                Detail = error.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            errorApiResponse.ErrorCode = (int)HttpStatusCode.NotFound;
            errorApiResponse.Title = Error.CantFind.Key;

            _logger.LogError(error, error.Message);

            var result = JsonSerializer.Serialize(errorApiResponse);
            await context.Response.WriteAsync(result);
        }
        catch (Exception error)
        {
            context.Response.ContentType = "application/json";

            var errorApiResponse = new ErrorApiResponse
            {
                Detail = error.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorApiResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
            errorApiResponse.Title = ErrorCodes.UnspecifiedError.ToString();

            _logger.LogError(error, error.Message);

            var result = JsonSerializer.Serialize(errorApiResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
