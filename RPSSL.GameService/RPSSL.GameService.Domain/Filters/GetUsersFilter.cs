using RPSSL.GameService.Domain.Filters.Common;

namespace RPSSL.GameService.Domain.Filters;

public record GetUsersFilter(string? FilterEmail, DateTime? CreatedFrom, DateTime? CreatedTo, bool? Active, int UserRole) : PaginatedFilter;
