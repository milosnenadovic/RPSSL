namespace RPSSL.GameService.Contracts._Common;

public abstract record BaseQueryStringParameters<T>
{
	const int maxPageSize = 100;
	public int PageNumber { get; set; } = 1;

	private int _pageSize = 10;
	public int PageSize
	{
		get
		{
			return _pageSize;
		}
		set
		{
			_pageSize = value > maxPageSize ? maxPageSize : value;
		}
	}

	public T? SortBy { get; set; }
	public bool SortDescending { get; set; }
}
