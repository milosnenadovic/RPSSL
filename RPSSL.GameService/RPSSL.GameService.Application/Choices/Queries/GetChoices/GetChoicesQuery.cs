using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Choices.Queries.GetChoices;

public record GetChoicesQuery(string? FilterName, bool? Active) : IRequest<IResponse<IEnumerable<GetChoicesQueryResponse>>>;
