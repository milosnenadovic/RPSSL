using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Localizations.Queries.GetLocalizationLabels;

public record GetLocalizationLabelsQuery(short LanguageId) : IRequest<IResponse<GetLocalizationLabelsQueryResponse>>;
