using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Domain.Models;

public class Language : BaseEntity
{
    public required string Name { get; set; }
    public required string LanguageCode { get; set; }
    public required string CountryId { get; set; }
}
