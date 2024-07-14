using Microsoft.Extensions.Options;
using RPSSL.GameService.Common.Configurations;
using System.Text;

namespace RPSSL.GameService.Middleware;

public class LogRequestsMiddleware(RequestDelegate next, Serilog.ILogger logger, IOptions<ServerSettings> serverSettings)
{
	private readonly RequestDelegate _next = next;
	private readonly Serilog.ILogger _logger = logger;
	private readonly ServerSettings _serverSettings = serverSettings.Value;
	private static readonly char[] separator = [',', ';'];

	public async Task InvokeAsync(HttpContext context)
	{
		context.Request.EnableBuffering();

		bool logUrl = _serverSettings.LogRequestUrls == "*"
			? context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
			: !string.IsNullOrEmpty(_serverSettings.LogRequestUrls) && (_serverSettings.LogRequestUrls ?? "")
				.Split(separator, StringSplitOptions.RemoveEmptyEntries)
				.Any(x => context.Request.Path.Value != null && context.Request.Path.Value.Contains(x.Trim(), StringComparison.OrdinalIgnoreCase));

		if (logUrl)
		{
			using var reader = new StreamReader(
				context.Request.Body,
				encoding: Encoding.UTF8,
				detectEncodingFromByteOrderMarks: false,
				bufferSize: 1024 * 1024,
				leaveOpen: true);
			var body = await reader.ReadToEndAsync();
			var headers = new StringBuilder();
			foreach (var h in context.Request.Headers)
			{
				headers.AppendLine($"{h.Key}:{h.Value};");
			}
			var url = $"{(context.Request.IsHttps ? "https" : "http")}://{context.Request?.Host.Value}{context.Request?.Path}";
			_logger
				.ForContext("Payload", body.Length > 50000 ? body.Substring(0, 50000) + body.Substring(body.Length - 50000) : body) // trim large request body due to logging
				.ForContext("Url", url)
				.ForContext("StatusCode", context.Response.StatusCode)
				.ForContext("Headers", headers)
				.Debug(@"Received {RequestMethod} {Url}{QueryString}", context.Request?.Method ?? string.Empty, url, context.Request?.QueryString.Value ?? string.Empty);
			if (context.Request is not null) 
				context.Request.Body.Position = 0;
		}

		await _next(context);
	}
}