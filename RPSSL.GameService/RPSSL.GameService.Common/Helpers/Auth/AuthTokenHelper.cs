using Microsoft.IdentityModel.Tokens;
using RPSSL.GameService.Common.Exceptions;
using RPSSL.GameService.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RPSSL.GameService.Common.Helpers.Auth;

public static class AuthTokenHelper
{
	public static async Task<bool> IsAdmin(string authToken)
	{
		var roleId = await ReadFromTokenPayload(authToken, TokenData.Role);
		if (roleId is null)
			return false;
		return int.Parse(roleId) == (int)Role.Admin;
	}

	public static async Task<string?> GetRoleId(string authToken)
	{
		return await ReadFromTokenPayload(authToken, TokenData.Role);
	}

	public static async Task<string?> GetUserId(string authToken)
	{
		return await ReadFromTokenPayload(authToken, TokenData.UserId);
	}

	public static async Task<string?> GetEmail(string authToken)
	{
		return await ReadFromTokenPayload(authToken, TokenData.Email);
	}

	public static string GenerateAuthToken(string email, string id, string userName, Role role, string secretKey, string issuer, string audience, short expiryMinutes)
	{
		var key = Encoding.UTF8.GetBytes(secretKey);

		var token = new JwtSecurityToken(
			issuer: issuer,
			audience: audience,
			claims:
			[
				new Claim(ClaimTypes.Email, email),
				new Claim(ClaimTypes.NameIdentifier, id),
				new Claim(ClaimTypes.Name, userName),
				new Claim(ClaimTypes.Role, EnumHelper.GetEnumDescription(role))
			],
			expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
			signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
		);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public static string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private static async Task<string?> ReadFromTokenPayload(string authToken, TokenData searchTokenData)
	{
		try
		{
			if (authToken.StartsWith("Bearer"))
				authToken = authToken.Replace("Bearer ", "");

			var searchTokenDataString = EnumHelper.GetEnumDescription(searchTokenData);

			var decodedJwtToken = await ReadTokenAsync(authToken);

			using JsonDocument document = JsonDocument.Parse(decodedJwtToken);
			JsonElement root = document.RootElement;
			JsonElement payload = root.GetProperty("Payload");

			var tokenData = payload.EnumerateArray().ToArray();

			var tokenValue = payload.EnumerateArray()
				.FirstOrDefault(x => x.GetProperty("Type").GetString() == searchTokenDataString)
				.GetProperty("Value").GetString();

			return tokenValue;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static async Task<string> ReadTokenAsync(string jwtInput)
	{
		return await Task.Run(() =>
		{
			return ReadToken(jwtInput);
		});
	}

	private static string ReadToken(string jwtInput)
	{
		var jwtHandler = new JwtSecurityTokenHandler();
		var jwtOutput = string.Empty;

		if (!jwtHandler.CanReadToken(jwtInput))
			throw new CantResolveUserException("The token doesn't seem to be in a proper JWT format.");

		JwtSecurityToken token;
		try
		{
			token = jwtHandler.ReadJwtToken(jwtInput);
		}
		catch (Exception ex)
		{
			throw new CantResolveUserException("Malformed JWT token.", ex);
		}

		var jwtHeader = JsonSerializer.Serialize(token.Header.Select(h => new { h.Key, h.Value }));
		jwtOutput = $"{{\r\n\"Header\":\r\n{JsonDocument.Parse(jwtHeader).RootElement},";

		var jwtPayload = JsonSerializer.Serialize(token.Claims.Select(c => new { c.Type, c.Value }));
		jwtOutput += $"\r\n\"Payload\":\r\n{JsonDocument.Parse(jwtPayload).RootElement}\r\n}}";

		return JsonDocument.Parse(jwtOutput).RootElement.ToString();
	}
}
