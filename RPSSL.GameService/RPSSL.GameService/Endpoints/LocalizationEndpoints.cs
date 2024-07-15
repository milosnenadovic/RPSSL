using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPSSL.GameService.Application.Localizations.Queries.GetLanguages;
using RPSSL.GameService.Application.Localizations.Queries.GetLocalizationLabels;
using RPSSL.GameService.Contracts.Localization;
using RPSSL.GameService.Endpoints.Base;

namespace RPSSL.GameService.Endpoints;

public class LocalizationEndpoints : BaseEndpoint, IEndpoints
{
	public static void DefineEndpoints(IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("Localization");

		#region Get
		group.MapGet("LocalizationLabels", GetLocalizationLabels)
			.WithTags("Localization");

		group.MapGet("Languages", GetLanguages)
			.WithTags("Localization");
		#endregion
	}

	public static void AddServices(IServiceCollection services, IConfiguration configuration)
	{
		throw new NotImplementedException();
	}

	#region GetLocalizationLabels
	public static async Task<IResult> GetLocalizationLabels([AsParameters] GetLocalizationLabelsRequest request, [FromServices] ISender sender)
	{
		var result = await sender.Send(new GetLocalizationLabelsQuery(request.LanguageId));

		if (result.IsSuccess)
			return Results.Ok(result.Result);

		return Results.Problem(
			detail: result.DetailedMessage,
			instance: nameof(result),
			statusCode: result.ErrorCode,
			title: result.Message);
	}
	#endregion

	#region GetLanguages
	public static async Task<IResult> GetLanguages([FromServices] ISender sender)
	{
		var result = await sender.Send(new GetLanguagesQuery());

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
