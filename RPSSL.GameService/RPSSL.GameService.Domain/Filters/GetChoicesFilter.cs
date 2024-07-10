namespace RPSSL.GameService.Domain.Filters;

public record GetChoicesFilter(string? PlayerId, DateTime? CreatedFrom, DateTime? CreatedTo, bool? Active);
