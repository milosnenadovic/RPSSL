using System.Text.Json.Serialization;

namespace RPSSL.GameService.Common.Response;

public class ErrorResponse : IResponse
{
    public bool IsSuccess => false;
    public string Message { get; set; }
    public string? DetailedMessage { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public int? ErrorCode { get; set; }

    public ErrorResponse(int? errorCode)
    {
        ErrorCode = errorCode;
        Message = "Error occured.";
    }

    public ErrorResponse(int? errorCode, string message, string? detailedMessage)
    {
        ErrorCode = errorCode;
        Message = message;
        DetailedMessage = detailedMessage;
    }

    public ErrorResponse(int? errorCode, Exception ex)
    {
        ErrorCode = errorCode;
        Message = ex.Message;

        if (ex.InnerException is var innerEx && innerEx is not null)
            DetailedMessage = innerEx.Message;
    }

    public static ErrorResponse Error(int? errorCode, string message)
        => new(errorCode, message, null);

    public static ErrorResponse<T> Error<T>(int? errorCode)
        => new(errorCode);

    public static ErrorResponse<T> Error<T>(int? errorCode, Exception ex)
        => new(errorCode, ex);

    public static ErrorResponse<T> Error<T>(int? errorCode, string message, string detailedMessage)
        => new(errorCode, message, detailedMessage);

    public static ErrorResponse<T> Error<T>(ErrorResponse response)
        => new(response.ErrorCode, response.Message, response.DetailedMessage);
}

public class ErrorResponse<T> : ErrorResponse, IResponse<T>
{
    public T Result { get; }

    public ErrorResponse(int? errorCode = null) : base(errorCode)
    {
    }

    public ErrorResponse(int? errorCode, string message, string? detailedMessage) : base(errorCode, message, detailedMessage)
    {
    }

    public ErrorResponse(int? errorCode, Exception ex) : base(errorCode, ex)
    {
    }
}
