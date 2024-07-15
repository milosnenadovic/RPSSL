using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Application.Users.Commands.LoginUser;

public record LoginUserCommandResponse(string Id, string Email, string Username, Role Role, string AuthToken);
