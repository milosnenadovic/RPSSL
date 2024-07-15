using Mapster;
using MediatR;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Filters;

namespace RPSSL.GameService.Application.Choices.Queries.GetChoices;

public class GetChoicesQueryHandler(IChoiceRepository choiceRepository) : IRequestHandler<GetChoicesQuery, IResponse<IEnumerable<GetChoicesQueryResponse>>>
{
	private readonly IChoiceRepository _choiceRepository = choiceRepository;

	public async Task<IResponse<IEnumerable<GetChoicesQueryResponse>>> Handle(GetChoicesQuery request, CancellationToken cancellationToken)
	{
		var choices = await _choiceRepository.GetChoices(new GetChoicesFilter(request.FilterName, request.Active), cancellationToken);

		if (choices is null)
			return new ErrorResponse<IEnumerable<GetChoicesQueryResponse>>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.LocalizationLabels);

		var response = choices.Adapt<IEnumerable<GetChoicesQueryResponse>>();

		return new SuccessResponse<IEnumerable<GetChoicesQueryResponse>>(response);
	}
}
