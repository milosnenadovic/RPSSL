using System.Text;
using System.Text.Json;

namespace RPSSL.Web.Helpers;

public static class ApiRequest
{
    public static async Task<HttpResponseMessage> Post(object? dto, string endPoint, HttpClient httpClient)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, endPoint);

        if (dto is not null)
        {
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(dto));
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        }

        var response = await httpClient.SendAsync(requestMessage);
        return response;
    }

    public static async Task<HttpResponseMessage> Get(Dictionary<string, string> queryParams, string endPoint, HttpClient httpClient)
    {
        if (queryParams != null && queryParams.Count > 0)
        {
            bool isFirst = true;
            foreach (var queryParam in queryParams)
            {
                if (isFirst)
                {
                    var sbFirst = new StringBuilder(endPoint);
                    sbFirst.Append("?");
                    endPoint = sbFirst.ToString();
                    isFirst = false;
                }
                else
                {
                    var sbNotFirst = new StringBuilder(endPoint);
                    sbNotFirst.Append("&");
                    endPoint = sbNotFirst.ToString();
                }
                var sb = new StringBuilder(endPoint);
                sb.Append(string.Format("{0}={1}", queryParam.Key, queryParam.Value));
                endPoint = sb.ToString();
            }
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, endPoint);

        var response = await httpClient.SendAsync(requestMessage);
        return response;
    }
}
