using RPSSL.GameService.Domain.Enums;
using RPSSL.GameService.Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPSSL.GameService.Domain.Models;

public class ChoicesHistory : BaseAuditableEntity
{
    public required string PlayerId { get; set; }
    public int PlayerChoiceId { get; set; }
    public int ComputerChoiceId { get; set; }

    [NotMapped]
    public GameResult Result { get; set; }
}
