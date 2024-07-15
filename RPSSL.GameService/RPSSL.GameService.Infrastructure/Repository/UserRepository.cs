using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Filters;
using RPSSL.GameService.Domain.Models;
using RPSSL.GameService.Infrastructure.Persistence;

namespace RPSSL.GameService.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
	#region Setup
	private readonly IApplicationDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;

	public UserRepository(IApplicationDbContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager)
		=> (_dbContext, _userManager, _signInManager) = (dbContext, userManager, signInManager);
	#endregion

	#region GetById
	/// <summary>
	/// Get user by id
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public async Task<User?> GetById(string id)
	{
		return await _userManager.FindByIdAsync(id);
	}
	#endregion

	#region GetUsers
	/// <summary>
	/// Get paginated users by email, userName, createdFrom, createdTo or active properties
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public async Task<KeyValuePair<IEnumerable<User>, int>> GetUsers(GetUsersFilter filter, CancellationToken cancellationToken = default)
	{
		var users = _dbContext.User
			.AsNoTracking();

		if (!string.IsNullOrEmpty(filter.FilterEmail))
			users = users.Where(x => x.Email!.Contains(filter.FilterEmail, StringComparison.CurrentCultureIgnoreCase));

		if (!string.IsNullOrEmpty(filter.FilterName))
			users = users.Where(x => x.UserName!.Contains(filter.FilterName, StringComparison.CurrentCultureIgnoreCase));

		if (filter.Active is not null)
			users = users.Where(x => x.Active == filter.Active);

		if (filter.CreatedFrom is not null)
			users = users.Where(x => x.Created >= filter.CreatedFrom);

		if (filter.CreatedTo is not null)
			users = users.Where(x => x.Created <= filter.CreatedTo);

		switch (filter.SortBy)
		{
			case 1:
				if (filter.SortDescending)
					users = users.OrderByDescending(x => x.Email);
				else
					users = users.OrderBy(x => x.Email);
				break;
			case 2:
				if (filter.SortDescending)
					users = users.OrderByDescending(x => x.UserName);
				else
					users = users.OrderBy(x => x.UserName);
				break;
			case 3:
				if (filter.SortDescending)
					users = users.OrderByDescending(x => x.Created);
				else
					users = users.OrderBy(x => x.Created);
				break;
			case 4:
				if (filter.SortDescending)
					users = users.OrderByDescending(x => x.Active);
				else
					users = users.OrderBy(x => x.Active);
				break;
			default:
				if (filter.SortDescending)
					users = users.OrderByDescending(x => x.Id);
				else
					users = users.OrderBy(x => x.Id);
				break;
		}

		if (!await users.AnyAsync(cancellationToken))
			return await Task.FromResult(new KeyValuePair<IEnumerable<User>, int>(Enumerable.Empty<User>(), 0));

		var count = await users.CountAsync(cancellationToken);

		if (filter.PageNumber > 0)
			users = users.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

		return await Task.FromResult(new KeyValuePair<IEnumerable<User>, int>(users.AsEnumerable(), count));
	}
	#endregion

	#region GetByEmail
	/// <summary>
	/// Get user by email
	/// </summary>
	/// <param name="email"></param>
	/// <returns></returns>
	public async Task<User?> GetByEmail(string email)
	{
		return await _userManager.FindByEmailAsync(email);
	}
	#endregion

	#region GetByUserName
	/// <summary>
	/// Get user by userName
	/// </summary>
	/// <param name="userName"></param>
	/// <returns></returns>
	public async Task<User?> GetByUserName(string userName)
	{
		return await _userManager.FindByNameAsync(userName);
	}
	#endregion

	#region CheckPassword
	/// <summary>
	/// Check user's password
	/// Interaction with DB using UserManager
	/// </summary>
	/// <param name="user"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public async Task<bool> CheckPassword(User user, string password)
	{
		return await _userManager.CheckPasswordAsync(user, password);
	}
	#endregion

	#region ChangePassword
	/// <summary>
	/// Change user's password
	/// Interaction with DB using UserManager
	/// </summary>
	/// <param name="user"></param>
	/// <param name="oldPassword"></param>
	/// <param name="newPassword"></param>
	/// <returns></returns>
	public async Task<bool> ChangePassword(User user, string oldPassword, string newPassword)
	{
		var changedPasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

		if (changedPasswordResult is null)
			return await Task.FromResult(false);

		if (!changedPasswordResult.Succeeded)
			return await Task.FromResult(false);

		return await Task.FromResult(true);
	}
	#endregion

	#region Add
	/// <summary>
	/// Add new user
	/// </summary>
	/// <param name="user"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public async Task<User?> Add(User user, string password)
	{
		var savedUser = await _userManager.CreateAsync(user, password);

		if (!savedUser.Succeeded)
			return null;

		return user;
	}
	#endregion

	#region Update
	/// <summary>
	/// Update user's info
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public async Task<bool> Update(User user)
	{
		var updateResult = await _userManager.UpdateAsync(user);

		if (updateResult is null)
			return await Task.FromResult(false);

		if (!updateResult.Succeeded)
			return await Task.FromResult(false);

		return await Task.FromResult(true);
	}
	#endregion

	#region Login
	/// <summary>
	/// Login user with password
	/// </summary>
	/// <param name="user"></param>
	/// <param name="password"></param>
	/// <param name="persistentLogin"></param>
	/// <returns></returns>
	public async Task<User?> Login(User user, string password, bool persistentLogin = false)
	{
		var loggedInUser = await _signInManager.PasswordSignInAsync(user, password, persistentLogin, lockoutOnFailure: false);
		if (loggedInUser is null)
			return null;
		if (!loggedInUser.Succeeded)
			return null;

		user.LastModifiedBy = "system";

		var updatedUser = await _userManager.UpdateAsync(user);
		if (updatedUser is null)
			return null;
		if (!updatedUser.Succeeded)
			return null;

		return user;
	}
	#endregion

	#region Logout
	/// <summary>
	/// Logout current user
	/// </summary>
	/// <returns></returns>
	public async Task Logout()
	{
		await _signInManager.SignOutAsync();
	}
	#endregion

	#region AddToRole
	/// <summary>
	/// Add new user to roles
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	public async Task<bool> AddToRole(User user)
	{
		var roleAdded = await _userManager.AddToRoleAsync(user, user.Role.ToString());

		if (!roleAdded.Succeeded)
			return await Task.FromResult(false);

		return await Task.FromResult(true);
	}
	#endregion
}
