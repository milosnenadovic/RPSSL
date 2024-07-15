using MediatR;
using Microsoft.Extensions.Logging;
using RPSSL.GameService.Common.Services;

namespace RPSSL.GameService.Application._Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger = logger;
	private readonly ICurrentUserService _currentUserService = currentUserService;
	private readonly List<string> _publicHandlers = ["GetLocalizationLabelsQuery", "LoginUserCommand", "GetLanguagesQuery", "PlayGameCommand"];

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;

		if (_publicHandlers.Contains(requestName))
			_logger.LogDebug("{Name} AccountId=null {@Request}", requestName, request);
		else
			_logger.LogDebug("{Name} {@AccountId} {@Request}", requestName, _currentUserService.CurrentUser?.Username ?? string.Empty, request);

		return await next.Invoke();
	}
}