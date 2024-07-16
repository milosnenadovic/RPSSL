using Microsoft.Extensions.Options;
using RPSSL.Web.Configurations;
using RPSSL.Web.Contracts._Common.Response;
using RPSSL.Web.Domain.Models;
using RPSSL.Web.Helpers;
using RPSSL.Web.Services.Abstractions;
using System.Text.Json;

namespace RPSSL.Web.Services;

public class LocalizationService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings) : ILocalizationService
{
    #region Setup
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    private readonly AppSettings _appSettings = appSettings.Value;
    #endregion

    #region GetLocalizationData
    /// <summary>
    /// Get localization data
    /// </summary>
    /// <returns></returns>
    public async Task<IResponse<LocalizedData>> GetLocalizationData(int languageId)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "LanguageId", languageId.ToString() }
        };

        HttpResponseMessage response = await ApiRequest.Get(queryParams, _appSettings.ApiSettings.LocalizationGetLocalizationData, _httpClient);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<LocalizedData>(responseBody);
            return new SuccessResponse<LocalizedData>(objectResult!);
        }
        else
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorApiResponse>(await response.Content.ReadAsStringAsync());
            return new ErrorResponse<LocalizedData>(errorResponse?.ErrorCode, errorResponse?.Title ?? ErrorCodes.UnspecifiedError.ToString(), errorResponse?.Detail ?? ErrorCodes.UnspecifiedError.ToString());
        }
    }
    #endregion
}
