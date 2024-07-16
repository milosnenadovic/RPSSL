using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Endpoints.Base;

public class BaseEndpoint
{
    public static IResult ConvertToResponse<T>(IResponse<T> response)
    {
        if (response.IsSuccess)
            return Results.Ok(response.Result);

        return Results.Problem(
            detail: response.DetailedMessage,
            instance: nameof(T),
            statusCode: response.ErrorCode,
            title: response.Message);
    }
}
