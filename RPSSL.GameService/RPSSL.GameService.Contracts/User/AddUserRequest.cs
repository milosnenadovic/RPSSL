﻿namespace RPSSL.GameService.Contracts.User;

public record AddUserRequest
{
	public required string Email { get; set; }
	public required string UserName { get; set; }
	public required string Password { get; set; }
	public string? Phone { get; set; }
}
