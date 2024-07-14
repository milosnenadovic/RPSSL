using Microsoft.AspNetCore.Antiforgery;
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
		catch (Exception error)
		{
			var response = context.Response;
			response.ContentType = "application/json";

			var errorApiResponse = new ErrorApiResponse
			{
				Detail = error.Message
			};

			switch (error)
			{
				case ValidationException ex:
					response.StatusCode = (int)HttpStatusCode.BadRequest;
					errorApiResponse.Title = Error.Validation.Middleware;
					errorApiResponse.ErrorCodes = [];
					foreach (var err in ex.Errors)
					{
						foreach (string errorValue in err.Value)
						{
							errorApiResponse.ErrorCodes.Add(new() { Code = err.Key, Description = errorValue });
							errorApiResponse.Detail = errorValue;
						}
					}
					break;
				case CantResolveUserException ex:
					response.StatusCode = (int)HttpStatusCode.Unauthorized;
					errorApiResponse.Title = Error.Authorization.CantResolveUser;
					break;
				case AntiforgeryValidationException ex:
					response.StatusCode = (int)HttpStatusCode.Unauthorized;
					errorApiResponse.Title = Error.Authorization.MissingToken;
					errorApiResponse.Detail = ex.InnerException?.Message ?? string.Empty;
					break;
				case KeyNotFoundException:
					response.StatusCode = (int)HttpStatusCode.NotFound;
					errorApiResponse.Title = Error.CantFind.Key;
					break;
				default:
					response.StatusCode = (int)HttpStatusCode.InternalServerError;
					errorApiResponse.Title = ErrorCodes.UnspecifiedError.ToString();
					break;
			}

			_logger.LogError(error, error.Message);

			var result = JsonSerializer.Serialize(errorApiResponse);
			await response.WriteAsync(result);
		}
	}
}
