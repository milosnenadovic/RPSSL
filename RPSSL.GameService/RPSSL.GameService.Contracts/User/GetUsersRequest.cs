using RPSSL.GameService.Contracts._Common;
using RPSSL.GameService.Contracts._Common.SortBy;

namespace RPSSL.GameService.Contracts.User;

public record GetUsersRequest : BaseQueryStringParameters<GetUsersSortBy>
{
	public string? FilterEmail { get; set; }
	public string? FilterName { get; set; }
	public DateTime? CreatedFrom { get; set; }
	public DateTime? CreatedTo { get; set; }
	public bool? Active { get; set; }
}
