namespace RPSSL.GameService.Common.Response;

public class SuccessResponse
{
    public bool IsSuccess => true;
    public string? Message { get; }
    public string? DetailedMessage { get; }
    public int? ErrorCode { get; }

    public static SuccessResponse Success()
        => new();

    public static SuccessResponse<T> Success<T>(T result)
        => new(result);
}

public class SuccessResponse<T>(T data) : SuccessResponse, IResponse<T>
{
    public T Result { get; set; } = data;
}
