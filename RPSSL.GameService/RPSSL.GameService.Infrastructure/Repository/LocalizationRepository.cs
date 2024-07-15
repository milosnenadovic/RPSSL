using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Infrastructure.Persistence;

namespace RPSSL.GameService.Infrastructure.Repository;

public class LocalizationRepository : ILocalizationRepository
{
	#region Setup
	private readonly IApplicationDbContext _dbContext;
	private readonly IMemoryCache _memoryCache;

	public LocalizationRepository(IApplicationDbContext dbContext, IMemoryCache memoryCache)
		=> (_dbContext, _memoryCache) = (dbContext, memoryCache);
	#endregion

	#region GetLocalizationLabels
	/// <summary>
	/// Get localization labels by countryId
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public async Task<IEnumerable<LocalizationLabel>?> GetLocalizationLabels(GetLocalizationsLabelFilter filter, CancellationToken cancellationToken = default)
	{
		string memoryCacheKey = $"GetLocalizationLabels-{filter.LanguageId}";

		if (!_memoryCache.TryGetValue(memoryCacheKey, out IEnumerable<LocalizationLabel>? labels))
		{
			labels = await _dbContext.LocalizationLabel
				.AsNoTracking()
				.Where(x => filter.LanguageId == null || x.LanguageId == filter.LanguageId)
				.ToListAsync(cancellationToken);

			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
			};

			_memoryCache.Set(memoryCacheKey, labels, cacheEntryOptions);
		}

		return labels;
	}
	#endregion

	#region GetLanguages
	/// <summary>
	/// Get all languages
	/// </summary>
	/// <returns></returns>
	public async Task<IEnumerable<Language>?> GetLanguages(CancellationToken cancellationToken = default)
	{
		string memoryCacheKey = "GetLanguages";

		if (!_memoryCache.TryGetValue(memoryCacheKey, out IEnumerable<Language>? languages))
		{
			languages = await _dbContext.Language
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
			};

			_memoryCache.Set(memoryCacheKey, languages, cacheEntryOptions);
		}

		return languages;
	}
	#endregion

	#region SaveLocalizationLabel
	/// <summary>
	/// Add new or update existing localization label
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	public async Task<bool> SaveLocalizationLabel(LocalizationLabel label, CancellationToken cancellationToken = default)
	{
		var dbLabel = await _dbContext.LocalizationLabel.SingleOrDefaultAsync(x => x.Key == label.Key && x.Language == label.Language, cancellationToken);

		if (dbLabel is null)
			_dbContext.LocalizationLabel.Add(label);
		else
			dbLabel.Value = label.Value;

		var languages = _dbContext.Language.AsNoTracking();

		foreach (var lang in languages)
		{
			var labels = _dbContext.LocalizationLabel
				.AsNoTracking()
				.Where(x => x.LanguageId == lang.Id);

			var setResult = _memoryCache.Set($"GetLocalizationLabels-{lang.Id}", await Task.FromResult(labels.AsEnumerable()));

			if (setResult is null)
				return await Task.FromResult(false);
		}

		return await Task.FromResult(true);
	}
	#endregion
}
