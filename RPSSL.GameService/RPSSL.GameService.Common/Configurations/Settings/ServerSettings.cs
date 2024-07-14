namespace RPSSL.GameService.Common.Configurations;

public class ServerSettings
{
	public const string SectionName = "ServerOptions";

	public required string AllowSwaggerCall { get; set; }
	public required string LogRequestUrls { get; set; }
}
