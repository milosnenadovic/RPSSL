using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface IChoiceRepository
{
	Task<IEnumerable<Choice>> GetChoices(GetChoicesFilter filter, CancellationToken cancellationToken = default);
	Task<Choice?> GetById(int id, CancellationToken cancellationToken = default);
	Task<bool> Add(string name, string image, ICollection<int>? choiceWins, CancellationToken cancellationToken = default);
	Task<bool> Update(string name, string image, ICollection<int>? choiceWins, CancellationToken cancellationToken = default);
}
