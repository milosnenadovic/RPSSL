using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface ILocalizationRepository
{
    Task<IEnumerable<LocalizationLabel>?> GetLocalizationLabels(GetLocalizationsLabelFilter filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<Language>?> GetLanguages(CancellationToken cancellationToken = default);
    Task<bool> SaveLocalizationLabel(LocalizationLabel label, CancellationToken cancellationToken = default);
}
