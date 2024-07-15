using System.ComponentModel;

namespace RPSSL.GameService.Domain.Enums;

public enum GameResult
{
	[Description("Win")]
	Win = 1,
	[Description("Lose")]
	Lose = 2,
	[Description("Tie")]
	Tie = 3
}
