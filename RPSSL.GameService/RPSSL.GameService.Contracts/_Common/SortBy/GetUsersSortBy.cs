using System.ComponentModel;

namespace RPSSL.GameService.Contracts._Common.SortBy;
public enum GetUsersSortBy
{
	[Description("Email")]
	Email = 1,
	[Description("Name")]
	Name = 2,
	[Description("Created")]
	Created = 3,
	[Description("Active")]
	Active = 4
}