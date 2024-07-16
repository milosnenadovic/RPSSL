using Mapster;
using MediatR;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Domain.Abstractions;

namespace RPSSL.GameService.Application.Localizations.Queries.GetLanguages;

public class GetLanguagesQueryHandler(ILocalizationRepository localizationService) : IRequestHandler<GetLanguagesQuery, IResponse<List<GetLanguagesQueryResponse>>>
{
    private readonly ILocalizationRepository _localizationService = localizationService;

    public async Task<IResponse<List<GetLanguagesQueryResponse>>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
        var languages = await _localizationService.GetLanguages(cancellationToken);

        if (languages is null)
            return new ErrorResponse<List<GetLanguagesQueryResponse>>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.Languages);

        var response = languages.Adapt<List<GetLanguagesQueryResponse>>();

        return new SuccessResponse<List<GetLanguagesQueryResponse>>(response);
    }
}
