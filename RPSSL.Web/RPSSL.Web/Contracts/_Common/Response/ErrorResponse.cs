using System.Text.Json.Serialization;

namespace RPSSL.Web.Contracts._Common.Response;

public class ErrorResponse : IResponse
{
    public bool IsSuccess => false;
    public string Message { get; set; }
    public string DetailedMessage { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public int? ErrorCode { get; set; }

    public ErrorResponse() { }

    public ErrorResponse(int? errorCode)
    {
        ErrorCode = errorCode;
    }

    public ErrorResponse(int? errorCode, string message, string detailedMessage = "")
    {
        ErrorCode = errorCode;
        Message = message;
        DetailedMessage = detailedMessage;
    }

    public ErrorResponse(int? errorCode, Exception ex)
    {
        ErrorCode = errorCode;
        Message = ex.Message;

        if (ex.InnerException is var innerEx && innerEx != null)
        {
            DetailedMessage = innerEx.Message;
        }
    }

    public static ErrorResponse Error(int? errorCode, string message)
        => new ErrorResponse(errorCode, message);

    public static ErrorResponse<T> Error<T>(int? errorCode)
        => new ErrorResponse<T>(errorCode);

    public static ErrorResponse<T> Error<T>(int? errorCode, Exception ex)
        => new ErrorResponse<T>(errorCode, ex);

    public static ErrorResponse<T> Error<T>(int? errorCode, string message, string detailedMessage = "")
        => new ErrorResponse<T>(errorCode, message, detailedMessage);

    public static ErrorResponse<T> Error<T>(ErrorResponse fireBlocksResponse)
        => new ErrorResponse<T>(fireBlocksResponse.ErrorCode, fireBlocksResponse.Message, fireBlocksResponse.DetailedMessage);
}

public class ErrorResponse<T> : ErrorResponse, IResponse<T>
{
    public T Result { get; }

    public ErrorResponse(int? errorCode = null) : base(errorCode)
    {
    }

    public ErrorResponse(int? errorCode, string message, string detailedMessage = "") : base(errorCode, message, detailedMessage)
    {
    }

    public ErrorResponse(int? errorCode, Exception ex) : base(errorCode, ex)
    {
    }
}
