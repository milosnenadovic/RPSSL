using Mapster;
using Microsoft.Extensions.DependencyInjection;
using RPSSL.GameService.Application.Localizations.Queries.GetLocalizationLabels;
using RPSSL.GameService.Application.Users.Commands.LoginUser;
using RPSSL.GameService.Domain.Models;
using System.Reflection;

namespace RPSSL.GameService.Application._Common.Mapping;

public static class MapsterConfig
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		//#region Contracts
		TypeAdapterConfig<IEnumerable<LocalizationLabel>, GetLocalizationLabelsQueryResponse>
			.NewConfig()
			.Map(dest => dest.LanguageId, src => src.First().LanguageId)
			.Map(dest => dest.LocalizationLabels, src => src);

		TypeAdapterConfig<User, LoginUserCommandResponse>
			.NewConfig()
			.Map(dest => dest.Username, src => src.UserName);

		//TypeAdapterConfig<Contract, GetContractsQueryResponse>
		//	.NewConfig()
		//	.Map(dest => dest.LicenseName, src => src.License.Name)
		//	.Map(dest => dest.OrganizationName, src => string.IsNullOrEmpty(src.Organization.Subdivision) ? src.Organization.Name : src.Organization.Name + " [" + src.Organization.Subdivision + "]");

		//TypeAdapterConfig<Contract, GetContractQueryResponse>
		//	.NewConfig()
		//	.Map(dest => dest.LicenseName, src => src.License.Name)
		//	.Map(dest => dest.OrganizationName, src => string.IsNullOrEmpty(src.Organization.Subdivision) ? src.Organization.Name : src.Organization.Name + " [" + src.Organization.Subdivision + "]");
		//#endregion

		TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
	}
}
