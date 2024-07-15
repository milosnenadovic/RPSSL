using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Localizations.Queries.GetLanguages;

public record GetLanguagesQuery : IRequest<IResponse<List<GetLanguagesQueryResponse>>>;
