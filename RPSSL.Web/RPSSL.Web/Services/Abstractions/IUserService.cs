using RPSSL.Web.Contracts._Common.Response;
using RPSSL.Web.Contracts.User;
using RPSSL.Web.Domain.Models;

namespace RPSSL.Web.Services.Abstractions;

public interface IUserService
{
    Task<IResponse<CurrentUser?>> Login(LoginUserRequest request);
    Task<IResponse<bool>> Logout();
    Task<IResponse<bool>> Register(RegisterUserRequest request);
}
