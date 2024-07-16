using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPSSL.GameService.Application.Choices.Commands.PlayGame;
using RPSSL.GameService.Application.Choices.Queries.GetChoices;
using RPSSL.GameService.Application.Choices.Queries.GetRandomChoice;
using RPSSL.GameService.Contracts.Choice;
using RPSSL.GameService.Endpoints.Base;

namespace RPSSL.GameService.Endpoints;

public class ChoiceEndpoints : BaseEndpoint, IEndpoints
{
    public static void DefineEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("Choice");

        #region Get
        group.MapGet("Choices", GetChoices)
            .WithTags("Choice");

        group.MapGet("Choice", GetRandomChoice)
            .WithTags("Choice");
        #endregion

        #region Post
        group.MapPost("Play", PlayGame)
            .WithTags("Choice");
        #endregion
    }

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    #region GetChoices
    public static async Task<IResult> GetChoices([AsParameters] GetChoicesRequest request, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetChoicesQuery(request.FilterName, request.Active));

        if (result.IsSuccess)
            return Results.Ok(result.Result);

        return Results.Problem(
            detail: result.DetailedMessage,
            instance: nameof(result),
            statusCode: result.ErrorCode,
            title: result.Message);
    }
    #endregion

    #region GetRandomChoice
    public static async Task<IResult> GetRandomChoice([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetRandomChoiceQuery());

        if (result.IsSuccess)
            return Results.Ok(result.Result);

        return Results.Problem(
            detail: result.DetailedMessage,
            instance: nameof(result),
            statusCode: result.ErrorCode,
            title: result.Message);
    }
    #endregion

    #region PlayGame
    public static async Task<IResult> PlayGame([FromBody] PlayGameRequest request, [FromServices] ISender sender)
    {
        var result = await sender.Send(new PlayGameCommand(request.PlayerChoiceId));

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
