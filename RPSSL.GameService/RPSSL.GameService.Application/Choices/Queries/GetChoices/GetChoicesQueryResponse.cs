namespace RPSSL.GameService.Application.Choices.Queries.GetChoices;

public record GetChoicesQueryResponse
{
	public short Id { get; set; }
	public required string Name { get; set; }
	public bool Active { get; set; }
	public List<ChoiceWinDto> ChoiceWins { get; set; } = [];
}

public record ChoiceWinDto(short BeatsChoiceId, string ActionName);