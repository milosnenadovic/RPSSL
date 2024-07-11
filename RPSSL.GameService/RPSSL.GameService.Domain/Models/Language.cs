namespace RPSSL.GameService.Domain.Models;

public class Language
{
	public short Id { get; set; }
	public required string Name { get; set; }
	public required string LanguageCode { get; set; }
	public required string CountryId { get; set; }
}
