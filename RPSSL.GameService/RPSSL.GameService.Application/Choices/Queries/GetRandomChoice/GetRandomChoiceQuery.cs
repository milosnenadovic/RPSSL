using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Choices.Queries.GetRandomChoice;

public record GetRandomChoiceQuery : IRequest<IResponse<GetRandomChoiceQueryResponse>>;
