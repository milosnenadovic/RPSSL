using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface IChoiceRepository
{
    Task<IEnumerable<Choice>> GetChoices(GetChoicesFilter filter, CancellationToken cancellationToken = default);
    Task<Choice?> GetById(int id, CancellationToken cancellationToken = default);
    void Add(Choice choice);
    Task<bool> Update(Choice choice, CancellationToken cancellationToken = default);
}
