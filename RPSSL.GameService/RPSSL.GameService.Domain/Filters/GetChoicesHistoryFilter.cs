using RPSSL.GameService.Domain.Filters.Common;

namespace RPSSL.GameService.Domain.Filters;

public record GetChoicesHistoryFilter(string? PlayerId, short? PlayerChoiceId, short? ComputerChoiceId, DateTime? CreatedFrom, DateTime? CreatedTo, bool? Active) : PaginatedFilter;
