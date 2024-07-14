namespace RPSSL.GameService.Common.Configurations.Settings;

public class ServicesSettings
{
	public const string SectionName = "Services";

	public ServiceSettings RandomGeneratedNumber { get; set; } = null!;
}

public class ServiceSettings
{
	public required string BaseUrl { get; set; }
	public required string Random { get; set; }
}
