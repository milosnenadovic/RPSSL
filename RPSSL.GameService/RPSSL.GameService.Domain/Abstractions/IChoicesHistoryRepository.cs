using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface IChoicesHistoryRepository
{
	Task<KeyValuePair<IEnumerable<ChoicesHistory>, int>> GetChoicesHistory(GetChoicesHistoryFilter filter, CancellationToken cancellationToken = default);
	Task<ChoicesHistory?> GetById(int id, CancellationToken cancellationToken = default);
	Task<bool> Add(string playerId, short playerChoiceId, short computerChoiceId, CancellationToken cancellationToken = default);
}
