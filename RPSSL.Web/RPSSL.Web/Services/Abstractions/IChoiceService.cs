using RPSSL.Web.Contracts._Common.Response;
using RPSSL.Web.Contracts.Choice;
using RPSSL.Web.Domain.Models;

namespace RPSSL.Web.Services.Abstractions;

public interface IChoiceService
{
    Task<IResponse<PlayGameResponse>> PlayGame(PlayGameRequest request);
    Task<IResponse<List<Choice>>> GetChoices(GetChoicesRequest request);
    Task<IResponse<Choice>> GetChoice();
}
