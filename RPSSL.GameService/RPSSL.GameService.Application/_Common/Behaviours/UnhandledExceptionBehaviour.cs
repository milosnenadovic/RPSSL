using MediatR;
using Microsoft.Extensions.Logging;

namespace RPSSL.GameService.Application._Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
	: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger = logger;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		try
		{
			return await next();
		}
		catch (Exception ex)
		{
			var requestName = typeof(TRequest).Name;

			_logger.LogError(ex, "RPSSL.GameService: Unhandled Exception for Request {Name} {@Request}", requestName, request);

			throw;
		}
	}
}