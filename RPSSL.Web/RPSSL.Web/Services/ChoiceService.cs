using Microsoft.Extensions.Options;
using RPSSL.Web.Configurations;
using RPSSL.Web.Contracts._Common.Response;
using RPSSL.Web.Contracts.Choice;
using RPSSL.Web.Domain.Models;
using RPSSL.Web.Helpers;
using RPSSL.Web.Services.Abstractions;
using System.Net;
using System.Text.Json;

namespace RPSSL.Web.Services;

public class ChoiceService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings) : IChoiceService
{
    #region Setup
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    private readonly AppSettings _appSettings = appSettings.Value;
    #endregion

    #region PlayGame
    /// <summary>
    /// Play game with chosen option
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IResponse<PlayGameResponse>> PlayGame(PlayGameRequest request)
    {
        HttpResponseMessage response = await ApiRequest.Post(request, _appSettings.ApiSettings.ChoicePlay, _httpClient);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<PlayGameResponse?>(responseBody);
            if (objectResult is null)
                return new ErrorResponse<PlayGameResponse>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), ErrorCodes.UnspecifiedError.ToString());
            else
                return new SuccessResponse<PlayGameResponse>(objectResult);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<PlayGameResponse>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion

    #region GetChoice
    /// <summary>
    /// Get random generated choice
    /// </summary>
    /// <returns></returns>
    public async Task<IResponse<Choice>> GetChoice()
    {
        var queryParams = new Dictionary<string, string>();

        HttpResponseMessage response = await ApiRequest.Get(queryParams, _appSettings.ApiSettings.ChoiceGetChoice, _httpClient);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<Choice?>(responseBody);
            if (objectResult is null)
                return new ErrorResponse<Choice>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), ErrorCodes.UnspecifiedError.ToString());
            else
                return new SuccessResponse<Choice>(objectResult);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<Choice>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion

    #region GetChoices
    /// <summary>
    /// Get list of choices
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IResponse<List<Choice>>> GetChoices(GetChoicesRequest request)
    {
        var queryParams = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(request.FilterName))
            queryParams.Add("FilterName", request.FilterName);

        if (request.Active is not null)
            queryParams.Add("Active", request.Active.ToString()!);

        HttpResponseMessage response = await ApiRequest.Get(queryParams, _appSettings.ApiSettings.ChoiceGetChoices, _httpClient);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<List<Choice>?>(responseBody);
            if (objectResult is null)
                return new ErrorResponse<List<Choice>>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), ErrorCodes.UnspecifiedError.ToString());
            else
                return new SuccessResponse<List<Choice>>(objectResult);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<List<Choice>>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion
}
