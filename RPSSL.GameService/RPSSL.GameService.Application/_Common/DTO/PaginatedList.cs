using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace RPSSL.GameService.Application._Common.DTO;

public class PaginatedList<T>
{
	public List<T> Items { get; }
	public int PageNumber { get; }
	public int TotalPages { get; }
	public int TotalCount { get; }

	[JsonConstructor]
	public PaginatedList(List<T> items, int totalCount, int pageNumber, int totalPages)
	{
		PageNumber = pageNumber;
		TotalPages = totalPages;
		TotalCount = totalCount;
		Items = items;
	}

	public PaginatedList(List<T> items, int pageNumber, int pageSize)
	{
		PageNumber = pageNumber;
		TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);
		TotalCount = items.Count;
		Items = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
	}

	public bool HasPreviousPage => PageNumber > 1;

	public bool HasNextPage => PageNumber < TotalPages;

	public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
	{
		var count = await source.CountAsync();
		var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

		return new PaginatedList<T>(items, count, pageNumber, pageSize);
	}
}
