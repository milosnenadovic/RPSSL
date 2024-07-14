using Microsoft.IdentityModel.Tokens;
using RPSSL.GameService.Common.Configurations;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Contracts._Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace RPSSL.GameService.Middleware;

public class JwtTokenValidationMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
{
	private readonly RequestDelegate next = next;
	private readonly TokenValidationParameters tokenValidationParameters = tokenValidationParameters;

	public async Task Invoke(HttpContext context)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var user = context.User;

		try
		{
			var token = context.Request.Headers[HttpContextItemKeys.Authorization];
			if (!string.IsNullOrEmpty(token))
			{
				if (token.ToString().Contains("Bearer"))
					token = token.ToString().Replace("Bearer ", "");
				if (token.ToString().Contains("Authorization"))
					token = token.ToString().Replace("Authorization ", "");
				tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
			}

			await next(context);
		}
		catch (SecurityTokenExpiredException ex)
		{
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			var errorEpiResponse = new ErrorApiResponse()
			{
				Detail = ex.Message,
				Title = ErrorCodes.Authorization.ToString(),
				ErrorCode = (int)ErrorCodes.Authorization,
				ErrorCodes = []
			};

			var result = JsonSerializer.Serialize(errorEpiResponse);
			await context.Response.WriteAsync(result);
		}
		catch (SecurityTokenInvalidSignatureException ex)
		{
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			var errorEpiResponse = new ErrorApiResponse()
			{
				Detail = ex.Message,
				Title = ErrorCodes.Authorization.ToString(),
				ErrorCode = (int)ErrorCodes.Authorization,
				ErrorCodes = []
			};

			var result = JsonSerializer.Serialize(errorEpiResponse);
			await context.Response.WriteAsync(result);
		}
		catch (Exception ex)
		{
			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			var errorEpiResponse = new ErrorApiResponse()
			{
				Detail = ex.Message,
				Title = ErrorCodes.Authorization.ToString(),
				ErrorCode = (int)ErrorCodes.Authorization,
				ErrorCodes = []
			};

			var result = JsonSerializer.Serialize(errorEpiResponse);
			await context.Response.WriteAsync(result);
		}
	}
}