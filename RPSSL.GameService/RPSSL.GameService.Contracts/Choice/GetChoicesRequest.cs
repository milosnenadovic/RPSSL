namespace RPSSL.GameService.Contracts.Choice;

public record GetChoicesRequest
{
	public string? FilterName { get; set; }
	public bool? Active { get; set; }
}
