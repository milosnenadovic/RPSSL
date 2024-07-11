using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface ILocalizationRepository
{
	Task<List<LocalizationLabel>> GetLocalizationLabels(CancellationToken cancellationToken = default);
}
