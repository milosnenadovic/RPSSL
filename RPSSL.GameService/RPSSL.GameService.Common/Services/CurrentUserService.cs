using RPSSL.GameService.Common.Constants.Errors;
using Microsoft.AspNetCore.Http;
using RPSSL.GameService.Common.Configurations;
using RPSSL.GameService.Common.Exceptions;
using RPSSL.GameService.Common.Helpers;
using RPSSL.GameService.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RPSSL.GameService.Common.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
	private CurrentUser _curUser;

	public CurrentUser CurrentUser
	{
		get
		{
			if (_curUser is null)
				SetPropertiesFromToken();

			return _curUser;
		}
	}

	void SetPropertiesFromToken()
	{
		var token = httpContextAccessor?.HttpContext?.Request.Headers[HttpContextItemKeys.Authorization].FirstOrDefault();

		if (string.IsNullOrEmpty(token)) throw new CantResolveUserException(Error.Authorization.MissingToken);

		if (token.StartsWith("bearer", StringComparison.CurrentCultureIgnoreCase)) token = token.Replace("Bearer ", string.Empty);

		var jwtoken = new JwtSecurityTokenHandler().ReadJwtToken(token);
		var claims = jwtoken.Claims.ToList();

		var userIdentity = new ClaimsIdentity(claims, "Id");
		if (httpContextAccessor?.HttpContext is not null)
		{
			httpContextAccessor.HttpContext.User = new ClaimsPrincipal(userIdentity);
			_curUser = new CurrentUser
			{
				UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(EnumHelper.GetEnumDescription(TokenData.UserId)),
				Role = (Role)int.Parse(httpContextAccessor.HttpContext?.User?.FindFirstValue(EnumHelper.GetEnumDescription(TokenData.Role))),
				Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(EnumHelper.GetEnumDescription(TokenData.Email)),
				Name = httpContextAccessor.HttpContext?.User?.FindFirstValue(EnumHelper.GetEnumDescription(TokenData.Name)),
				AuthToken = token
			};
			_ = Enum.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(EnumHelper.GetEnumDescription(TokenData.Role)), out Role curUserRole);
			_curUser.Role = curUserRole;
		}
	}
}
