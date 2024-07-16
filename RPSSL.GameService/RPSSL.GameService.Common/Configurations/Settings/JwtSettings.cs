using System.ComponentModel.DataAnnotations;

namespace RPSSL.GameService.Common.Configurations.Settings;

public class JwtSettings
{
    public const string SectionName = "JWTSettings";
    public required string SecretKey { get; set; }
    public short ExpiryMinutes { get; set; }
    [Required(ErrorMessage = "JWTSettings.Issuer is required.")]
    public required string Issuer { get; set; }
    [Required(ErrorMessage = "JwtSettings.Audience is required.")]
    public required string Audience { get; set; }
}
