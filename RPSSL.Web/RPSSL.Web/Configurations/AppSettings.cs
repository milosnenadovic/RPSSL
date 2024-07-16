namespace RPSSL.Web.Configurations;

public class AppSettings
{
    public required ApiSettings ApiSettings { get; set; }
    public int AuthExpirationMinutes { get; set; }
}

public class ApiSettings
{
    public required string BaseUrl { get; set; }

    #region User API
    public required string UserRegister { get; set; }
    public required string UserLogin { get; set; }
    public required string UserLogout { get; set; }
    public required string UserUpdate { get; set; }
    public required string UserGetUser { get; set; }
    public required string UserGetUsers { get; set; }
    #endregion

    #region Choice API
    public required string ChoiceGetChoice { get; set; }
    public required string ChoiceGetChoices { get; set; }
    public required string ChoicePlay { get; set; }
    public required string ChoiceAdd { get; set; }
    public required string ChoiceUpdate { get; set; }
    #endregion

    #region Localization
    public required string LocalizationGetLocalizationData { get; set; }
    #endregion
}
