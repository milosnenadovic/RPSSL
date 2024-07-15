using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Choices.Commands.PlayGame;

public record PlayGameCommand(short PlayerChoiceId) : IRequest<IResponse<PlayGameCommandResponse>>;
