using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPSSL.GameService.Application.Users.Commands.LoginUser;
using RPSSL.GameService.Application.Users.Commands.LogoutUser;
using RPSSL.GameService.Application.Users.Commands.RegisterUser;
using RPSSL.GameService.Configurations.Identity;
using RPSSL.GameService.Contracts.User;
using RPSSL.GameService.Endpoints.Base;

namespace RPSSL.GameService.Endpoints;

public class UserEndpoints : BaseEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("User");

        #region Get
        // todo: create queries
        //group.MapGet("Users", GetUsers)
        //	.WithTags("User");

        //group.MapGet("User", GetUser)
        //	.WithTags("User");
        #endregion

        #region Post
        group.MapPost("Register", Register)
            .WithTags("User");

        group.MapPost("Login", Login)
            .WithTags("User");

        group.MapPost("Logout", Logout)
            .RequireAuthorization(IdentityData.PolicyAdminUser)
            .WithTags("User");

        // todo: create command
        //group.MapPost("Update", Update)
        //	.RequireAuthorization(IdentityData.PolicyAdminUser)
        //	.WithTags("User");
        #endregion
    }

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }


    #region GetUser
    //public static async Task<IResult> GetUser([AsParameters] int id, [FromServices] ISender sender)
    //{
    //	var result = await sender.Send(new GetUserQuery(request.Id));

    //	if (result.IsSuccess)
    //		return Results.Ok(result.Result);

    //	return Results.Problem(
    //		detail: result.DetailedMessage,
    //		instance: nameof(result),
    //		statusCode: result.ErrorCode,
    //		title: result.Message);
    //}
    #endregion

    #region Register
    public static async Task<IResult> Register([FromBody] RegisterUserRequest request, [FromServices] ISender sender)
    {
        var command = request.Adapt<RegisterUserCommand>();
        var result = await sender.Send(command);

        if (result.IsSuccess)
            return Results.Ok(result.Result);

        return Results.Problem(
            detail: result.DetailedMessage,
            instance: nameof(result),
            statusCode: result.ErrorCode,
            title: result.Message);
    }
    #endregion

    #region Update
    //public static async Task<IResult> Update([FromBody] UpdateUserRequest request, [FromServices] ISender sender)
    //{
    //	var command = request.Adapt<UpdateUserCommand>();
    //	var result = await sender.Send(command);

    //	if (result.IsSuccess)
    //		return Results.Ok(result.Result);

    //	return Results.Problem(
    //		detail: result.DetailedMessage,
    //		instance: nameof(result),
    //		statusCode: result.ErrorCode,
    //		title: result.Message);
    //}
    #endregion

    #region Login
    protected static async Task<IResult> Login([FromBody] LoginUserRequest request, [FromServices] ISender sender)
    {
        var command = request.Adapt<LoginUserCommand>();
        var result = await sender.Send(command);

        if (result.IsSuccess)
            return Results.Ok(result.Result);

        return Results.Problem(
            detail: result.DetailedMessage,
            instance: nameof(result),
            statusCode: result.ErrorCode,
            title: result.Message);
    }
    #endregion

    #region Logout
    public static async Task<IResult> Logout([FromServices] ISender sender)
    {
        var result = await sender.Send(new LogoutUserCommand());

        if (result.IsSuccess)
            return Results.Ok(result.Result);

        return Results.Problem(
            detail: result.DetailedMessage,
            instance: nameof(result),
            statusCode: result.ErrorCode,
            title: result.Message);
    }
    #endregion
}
