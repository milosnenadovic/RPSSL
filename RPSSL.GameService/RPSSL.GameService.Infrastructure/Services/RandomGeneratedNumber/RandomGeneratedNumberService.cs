using Microsoft.Extensions.Options;
using RPSSL.GameService.Common.Configurations.Settings;
using RPSSL.GameService.Domain.Abstractions;
using System.Net;
using System.Text.Json;

namespace RPSSL.GameService.Infrastructure.Services.RandomGeneratedNumber;

public class RandomGeneratedNumberService(IHttpClientFactory httpClientFactory, IOptions<ServicesSettings> appSettings) : IRandomGeneratedNumberService
{
    #region Setup
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RandomGeneratedNumberAPI");
    private readonly ServicesSettings _appSettings = appSettings.Value;
    #endregion

    #region GetRandomNumber
    /// <summary>
    /// Get random integer 1-100
    /// </summary>
    /// <returns></returns>
    public async Task<short> GetRandomNumber()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, _appSettings.RandomGeneratedNumber.Random);

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var objectResult = JsonSerializer.Deserialize<RandomGeneratedNumberResponse>(responseBody);
            return objectResult?.RandomNumber ?? 0;
        }
        else
        {
            return -1;
        }
    }
    #endregion
}
