namespace RPSSL.GameService.Domain.Filters.Common;

public abstract record PaginatedFilter
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public int SortBy { get; set; }
	public bool SortDescending { get; set; }
}
