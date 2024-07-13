using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RPSSL.GameService.Application._Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
	: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly Stopwatch _timer = new Stopwatch();
	private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger = logger;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();

		var response = await next();

		_timer.Stop();

		var elapsedMilliseconds = _timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 100)
		{
			var requestName = typeof(TRequest).Name;

			_logger.LogInformation("RPSSL.GameService Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
				requestName, elapsedMilliseconds, request);
		}

		return response;
	}
}
