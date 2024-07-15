using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface IChoicesHistoryRepository
{
	Task<KeyValuePair<IEnumerable<ChoicesHistory>, int>> GetChoicesHistory(GetChoicesHistoryFilter filter, CancellationToken cancellationToken = default);
	void Add(ChoicesHistory choicesHistory, CancellationToken cancellationToken = default);
	Task DeleteAll(string userId, CancellationToken cancellationToken = default);
}
