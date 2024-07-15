using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Users.Commands.LogoutUser;

public record LogoutUserCommand : IRequest<IResponse<bool>>;
