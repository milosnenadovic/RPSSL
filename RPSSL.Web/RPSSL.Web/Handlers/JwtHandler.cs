using RPSSL.Web.Auth;
using RPSSL.Web.Configurations;
using System.Net.Http.Headers;

namespace RPSSL.Web.Handlers;

public class JwtHandler : DelegatingHandler
{
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

    public JwtHandler(CustomAuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var token = authState?.User.FindFirst(CustomClaimTypes.AuthToken)?.Value;

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue(HttpContextItemKeys.Bearer, token);

        return await base.SendAsync(request, cancellationToken);
    }
}
