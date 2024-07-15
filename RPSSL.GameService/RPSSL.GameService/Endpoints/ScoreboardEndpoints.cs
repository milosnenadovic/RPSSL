using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPSSL.GameService.Application.Scoreboard.Commands.ResetScoreboard;
using RPSSL.GameService.Application.Scoreboard.Queries.GetScoreboard;
using RPSSL.GameService.Configurations.Identity;
using RPSSL.GameService.Contracts.Scoreboard;
using RPSSL.GameService.Endpoints.Base;

namespace RPSSL.GameService.Endpoints;

public class ScoreboardEndpoints : BaseEndpoint, IEndpoints
{
	public static void DefineEndpoints(IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("Scoreboard");

		#region Get
		group.MapGet("Scoreboard", GetScoreboard)
			.RequireAuthorization(IdentityData.PolicyAdminUser)
			.WithTags("Scoreboard");
		#endregion

		#region Post
		group.MapPost("Reset", ResetScoreboard)
			.RequireAuthorization(IdentityData.PolicyAdminUser)
			.WithTags("Scoreboard");
		#endregion
	}

	public static void AddServices(IServiceCollection services, IConfiguration configuration)
	{
		throw new NotImplementedException();
	}

	#region GetScoreboard
	public static async Task<IResult> GetScoreboard([AsParameters] GetScoreboardRequest request, [FromServices] ISender sender)
	{
		var query = request.Adapt<GetScoreboardQuery>();
		var result = await sender.Send(query);

		if (result.IsSuccess)
			return Results.Ok(result.Result);

		return Results.Problem(
			detail: result.DetailedMessage,
			instance: nameof(result),
			statusCode: result.ErrorCode,
			title: result.Message);
	}
	#endregion

	#region ResetScoreboard
	public static async Task<IResult> ResetScoreboard([FromServices] ISender sender)
	{
		var result = await sender.Send(new ResetScoreboardCommand());

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
