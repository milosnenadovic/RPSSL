using Mapster;
using RPSSL.GameService.Application.Scoreboard.Queries.GetScoreboard;
using RPSSL.GameService.Contracts.Scoreboard;
using RPSSL.GameService.Domain.Enums;
using System.Reflection;

namespace RPSSL.GameService.Configurations.Mapping;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        #region Scoreboard
        TypeAdapterConfig<GetScoreboardRequest, GetScoreboardQuery>
            .NewConfig()
            .MapWith(src => new GetScoreboardQuery(
                src.PlayerChoiceId,
                src.ComputerChoiceId,
                (GameResult?)src.Result,
                src.PlayedFrom,
                src.PlayedTo)
            {
                PageNumber = src.PageNumber,
                PageSize = src.PageSize,
                SortBy = src.SortBy,
                SortDescending = src.SortDescending
            });
        #endregion

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
