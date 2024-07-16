using Mapster;
using MediatR;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Domain.Abstractions;

namespace RPSSL.GameService.Application.Choices.Queries.GetRandomChoice;

public class GetRandomChoiceQueryHandler(IChoiceRepository choiceRepository, IRandomGeneratedNumberService randomGeneratedNumberService) : IRequestHandler<GetRandomChoiceQuery, IResponse<GetRandomChoiceQueryResponse>>
{
    private readonly IChoiceRepository _choiceRepository = choiceRepository;
    private readonly IRandomGeneratedNumberService _randomGeneratedNumberService = randomGeneratedNumberService;

    public async Task<IResponse<GetRandomChoiceQueryResponse>> Handle(GetRandomChoiceQuery request, CancellationToken cancellationToken)
    {
        var id = await _randomGeneratedNumberService.GetRandomNumber();

        if (id < 1 || id > 100)
            return new ErrorResponse<GetRandomChoiceQueryResponse>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), Error.CantFind.Choice);

        var choices = await _choiceRepository.GetById((id % 5 + 1), cancellationToken);

        if (choices is null)
            return new ErrorResponse<GetRandomChoiceQueryResponse>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.Choice);

        var response = choices.Adapt<GetRandomChoiceQueryResponse>();

        return new SuccessResponse<GetRandomChoiceQueryResponse>(response);
    }
}
