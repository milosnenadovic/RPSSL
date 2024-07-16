using MediatR;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Common.Services;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Enums;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Application.Choices.Commands.PlayGame;

public class PlayGameCommandHandler(IChoicesHistoryRepository choicesHistoryRepository, IChoiceWinRepository choiceWinRepository,
    ICurrentUserService currentUserService, IRandomGeneratedNumberService randomGeneratedNumberService, IUserRepository userRepository)
    : IRequestHandler<PlayGameCommand, IResponse<PlayGameCommandResponse>>
{
    private readonly IChoicesHistoryRepository _choicesHistoryRepository = choicesHistoryRepository;
    private readonly IChoiceWinRepository _choiceWinRepository = choiceWinRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IRandomGeneratedNumberService _randomGeneratedNumberService = randomGeneratedNumberService;

    public async Task<IResponse<PlayGameCommandResponse>> Handle(PlayGameCommand request, CancellationToken cancellationToken)
    {
        User? user = null;

        if (_currentUserService.CurrentUser?.UserId is not null)
        {
            user = await _userRepository.GetById(_currentUserService.CurrentUser.UserId);
            if (user is null && _currentUserService.CurrentUser.UserId is not null)
                return new ErrorResponse<PlayGameCommandResponse>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.User);
        }

        var random = await _randomGeneratedNumberService.GetRandomNumber();
        if (random < 1 || random > 100)
            return new ErrorResponse<PlayGameCommandResponse>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), Error.CantFind.Choice);

        short computerChoiceId = (short)(random % 5 + 1);

        if (user is not null)
        {
            var newRecord = new ChoicesHistory
            {
                PlayerId = user.Id,
                PlayerChoiceId = request.PlayerChoiceId,
                ComputerChoiceId = computerChoiceId,
                CreatedBy = "system",
                Active = true
            };
            _choicesHistoryRepository.Add(newRecord, cancellationToken);
        }

        GameResult gameResult;

        if (request.PlayerChoiceId == computerChoiceId)
            gameResult = GameResult.Tie;
        else
        {
            var wins = await _choiceWinRepository.GetChoiceWins(request.PlayerChoiceId, cancellationToken);
            if (wins is null)
                return new ErrorResponse<PlayGameCommandResponse>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.ChoiceWins);

            if (wins.Any(x => x.BeatsChoiceId == computerChoiceId))
                gameResult = GameResult.Win;
            else
                gameResult = GameResult.Lose;
        }

        var returnResult = new PlayGameCommandResponse(gameResult.ToString(), request.PlayerChoiceId, computerChoiceId);

        return new SuccessResponse<PlayGameCommandResponse>(returnResult);
    }
}
