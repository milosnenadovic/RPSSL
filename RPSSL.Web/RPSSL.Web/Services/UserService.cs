using Microsoft.Extensions.Options;
using RPSSL.Web.Auth;
using RPSSL.Web.Configurations;
using RPSSL.Web.Contracts._Common.Response;
using RPSSL.Web.Contracts.User;
using RPSSL.Web.Domain.Models;
using RPSSL.Web.Helpers;
using RPSSL.Web.Services.Abstractions;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RPSSL.Web.Services;

public class UserService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, CustomAuthenticationStateProvider authenticationStateProvider) : IUserService
{
    #region Setup
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
    #endregion

    #region Login
    /// <summary>
    /// Login user with credentials
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IResponse<CurrentUser?>> Login(LoginUserRequest request)
    {
        HttpResponseMessage response = await ApiRequest.Post(request, _appSettings.ApiSettings.UserLogin, _httpClient);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<CurrentUser>(responseBody);
            if (objectResult is not null)
            {
                await _authenticationStateProvider.Login(objectResult);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(HttpContextItemKeys.Bearer, objectResult.AuthToken);
            }
            return new SuccessResponse<CurrentUser?>(objectResult ?? null);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<CurrentUser?>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion

    #region Logout
    /// <summary>
    /// Logout user
    /// </summary>
    /// <returns></returns>
    public async Task<IResponse<bool>> Logout()
    {
        var queryParams = new Dictionary<string, string>();

        HttpResponseMessage response = await ApiRequest.Get(queryParams, _appSettings.ApiSettings.UserLogout, _httpClient);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<bool>(responseBody);
            if (objectResult)
            {
                await _authenticationStateProvider.Logout();
                _httpClient.DefaultRequestHeaders.Remove(HttpContextItemKeys.Authorization);
            }
            return new SuccessResponse<bool>(objectResult);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<bool>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion

    #region Register
    /// <summary>
    /// Register as a new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IResponse<bool>> Register(RegisterUserRequest request)
    {
        HttpResponseMessage response = await ApiRequest.Post(request, _appSettings.ApiSettings.UserRegister, _httpClient);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<bool>(responseBody);
            return new SuccessResponse<bool>(objectResult);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<bool>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion
}
