using Mapster;
using MediatR;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Filters;

namespace RPSSL.GameService.Application.Localizations.Queries.GetLocalizationLabels;

public class GetLocalizationLabelsQueryHandler(ILocalizationRepository localizationService) : IRequestHandler<GetLocalizationLabelsQuery, IResponse<GetLocalizationLabelsQueryResponse>>
{
	private readonly ILocalizationRepository _localizationService = localizationService;

	public async Task<IResponse<GetLocalizationLabelsQueryResponse>> Handle(GetLocalizationLabelsQuery request, CancellationToken cancellationToken)
	{
		var localizationLabels = await _localizationService.GetLocalizationLabels(new GetLocalizationsLabelFilter(request.LanguageId), cancellationToken);

		if (localizationLabels is null)
			return new ErrorResponse<GetLocalizationLabelsQueryResponse>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.LocalizationLabels);

		var response = localizationLabels.Adapt<GetLocalizationLabelsQueryResponse>();

		return new SuccessResponse<GetLocalizationLabelsQueryResponse>(response);
	}
}
