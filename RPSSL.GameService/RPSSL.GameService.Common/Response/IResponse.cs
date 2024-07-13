namespace RPSSL.GameService.Common.Response;

public interface IResponse
{
	bool IsSuccess { get; }
	string? Message { get; }
	string? DetailedMessage { get; }
	int? ErrorCode { get; }

	public string FormatLogMessage()
		=> $"Message: {Message}"
		   + $"{Environment.NewLine}Detailed message: {DetailedMessage}"
		   + (IsSuccess ? string.Empty : $"{Environment.NewLine}Error code: {ErrorCode}");
}

public interface IResponse<T> : IResponse
{
	T Result { get; }
}
