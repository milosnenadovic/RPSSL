using RPSSL.Web.Contracts._Common.Response;
using RPSSL.Web.Domain.Models;

namespace RPSSL.Web.Services.Abstractions;

public interface ILocalizationService
{
    Task<IResponse<LocalizedData>> GetLocalizationData(int languageId);
}
