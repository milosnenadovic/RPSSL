using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface IChoiceWinRepository
{
	Task<IEnumerable<ChoiceWin>> GetChoiceWins(int? choiceId, CancellationToken cancellationToken = default);
}
