using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Username, string Email, string Password, string? Phone) : IRequest<IResponse<bool>>;
