using RPSSL.GameService.Domain.Filters.Common;

namespace RPSSL.GameService.Domain.Filters;

public record GetUsersFilter(string? FilterEmail, string? FilterName, DateTime? CreatedFrom, DateTime? CreatedTo, bool? Active) : PaginatedFilter;
