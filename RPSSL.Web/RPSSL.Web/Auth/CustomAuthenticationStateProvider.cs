using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RPSSL.Web.Configurations;
using RPSSL.Web.Domain.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RPSSL.Web.Auth;

public class CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService) : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorageService = sessionStorageService;
    private CurrentUser? CurrentUser { get; set; }

    public async Task Login(CurrentUser user)
    {
        CurrentUser = user;
        await _sessionStorageService.SaveItemEncryptedAsync(HttpContextItemKeys.AuthToken, user.AuthToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        CurrentUser = null;
        await _sessionStorageService.RemoveItemAsync(HttpContextItemKeys.AuthToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<CurrentUser?> GetCurrentUser()
    {
        await GetAuthenticationStateAsync();
        return CurrentUser;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        if (CurrentUser is null)
        {
            var token = await _sessionStorageService.ReadEncryptedItemAsync(HttpContextItemKeys.AuthToken);
            if (!string.IsNullOrEmpty(token))
            {
                var parsedClaims = ParseClaimsFromJwt(token);
                if (parsedClaims is not null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Email, parsedClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty),
                        new(ClaimTypes.NameIdentifier, parsedClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty),
                        new(ClaimTypes.Name, parsedClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty),
                        new(ClaimTypes.Role, parsedClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty),
                        new(CustomClaimTypes.AuthToken, token),
                        new(CustomClaimTypes.Expiration, parsedClaims.FirstOrDefault(x => x.Type == CustomClaimTypes.Expiration)?.Value ?? string.Empty)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "JwtAuth");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    state = new AuthenticationState(claimsPrincipal);
                }
            }
        }

        if (CurrentUser is not null)
        {
            var token = await _sessionStorageService.ReadEncryptedItemAsync(HttpContextItemKeys.AuthToken);
            if (!string.IsNullOrEmpty(token))
            {
                var parsedClaims = ParseClaimsFromJwt(token);
                if (parsedClaims is not null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Email, CurrentUser.Email),
                        new(ClaimTypes.NameIdentifier, CurrentUser.Id.ToString()),
                        new(ClaimTypes.Name, CurrentUser.Username),
                        new(ClaimTypes.Role, CurrentUser.Role.ToString()),
                        new(CustomClaimTypes.AuthToken, CurrentUser.AuthToken),
                        new(CustomClaimTypes.Expiration, parsedClaims!.FirstOrDefault(x => x.Type == CustomClaimTypes.Expiration)?.Value ?? string.Empty)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "JwtAuth");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    state = new AuthenticationState(claimsPrincipal);
                }
            }
        }

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var bytesAsString = Encoding.UTF8.GetString(jsonBytes);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(bytesAsString);
        return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}