namespace RPSSL.GameService.Infrastructure.Settings;

public class DatabaseSettings
{
	public const string SectionName = "ConnectionStrings";
	public required string DefaultConnection { get; set; }
}
