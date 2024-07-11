using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Domain.Abstractions;

public interface IUserRepository
{
	Task<KeyValuePair<IEnumerable<User>, int>> GetUsers(GetUsersFilter filter, CancellationToken cancellationToken = default);
	Task<User> GetById(string id, CancellationToken cancellationToken = default);
	Task<User?> Add(User user, CancellationToken cancellationToken = default);
	Task<bool> Update(User user, CancellationToken cancellationToken = default);
	Task<User?> Login(User user, string password, bool rememberMe, CancellationToken cancellationToken = default);
	Task Logout(CancellationToken cancellationToken = default);
	Task<bool> CheckPassword(User user, string password, CancellationToken cancellationToken = default);
	Task<bool> ChangePassword(User user, string oldPassword, string newPassword, CancellationToken cancellationToken = default);
}
