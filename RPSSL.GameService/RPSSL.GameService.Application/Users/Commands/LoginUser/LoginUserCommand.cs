using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Username, string Password, bool PersistentLogin) : IRequest<IResponse<LoginUserCommandResponse>>;
