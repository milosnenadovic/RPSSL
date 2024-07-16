using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence;

public sealed class ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, IApplicationDbContext context,
    UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        await SeedLanguages();
        await SeedChoices();
        await SeedLabels();
        await SeedRoles();
        await SeedUsers();
    }

    #region Choice
    private async Task SeedChoices()
    {
        #region List of choices to seed
        var choices = new List<Choice>
        {
            new()
            {
                Name = "Rock",
                Active = true,
                ChoiceWins = [
                    new() { BeatsChoiceId = 5, ActionName = "Crushes", Active = true },
                    new() { BeatsChoiceId = 3, ActionName = "Crushes", Active = true }
                    ]
            },
            new()
            {
                Name = "Paper",
                Active = true,
                ChoiceWins = [
                    new() { BeatsChoiceId = 1, ActionName = "Covers", Active = true },
                    new() { BeatsChoiceId = 4, ActionName = "Disproves", Active = true }
                    ]
            },
            new()
            {
                Name = "Scissors",
                Active = true,
                ChoiceWins = [
                    new() { BeatsChoiceId = 2, ActionName = "Cuts", Active = true },
                    new() { BeatsChoiceId = 5, ActionName = "Decapitates", Active = true }
                    ]
            },
            new()
            {
                Name = "Spock",
                Active = true,
                ChoiceWins = [
                    new() { BeatsChoiceId = 3, ActionName = "Smashes", Active = true },
                    new() { BeatsChoiceId = 1, ActionName = "Vaporizes", Active = true }
                    ]
            },
            new()
            {
                Name = "Lizard",
                Active = true,
                ChoiceWins = [
                    new() { BeatsChoiceId = 4, ActionName = "Poisons", Active = true },
                    new() { BeatsChoiceId = 2, ActionName = "Eats", Active = true }
                    ]
            }
        };
        #endregion

        foreach (var choice in choices)
            SeedChoice(choice);

        void SeedChoice(Choice choice)
        {
            var dbLanguage = _context.Choice.FirstOrDefault(x => x.Name == choice.Name);

            if (dbLanguage is null)
                _context.Choice.Add(choice);
        }

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Localization

    #region Languages
    private async Task SeedLanguages()
    {
        #region List of languages to seed
        var languages = new List<Language>
        {
            new() { Name = "English", LanguageCode = "en-US", CountryId = "US" },
            new() { Name = "Srpski", LanguageCode = "sr-Latn-RS", CountryId = "RS" }
        };
        #endregion

        foreach (var language in languages)
            SeedLanguage(language);

        void SeedLanguage(Language language)
        {
            var dbLanguage = _context.Language.FirstOrDefault(x => x.LanguageCode == language.LanguageCode);

            if (dbLanguage is null) _context.Language.Add(language);
            else
            {
                dbLanguage.Name = language.Name;
                dbLanguage.CountryId = language.CountryId;
            }
        }

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Labels
    private async Task SeedLabels()
    {
        #region sr-Latn-RS

        #region Labels
        SeedLabel(new LocalizationLabel() { Key = "Login", LanguageId = 2, Value = "Prijava" });
        SeedLabel(new LocalizationLabel() { Key = "Logout", LanguageId = 2, Value = "Odjava" });
        SeedLabel(new LocalizationLabel() { Key = "Ok", LanguageId = 2, Value = "Ok" });
        SeedLabel(new LocalizationLabel() { Key = "Choose", LanguageId = 2, Value = "Odaberi opciju" });
        SeedLabel(new LocalizationLabel() { Key = "Scoreboard", LanguageId = 2, Value = "Rezultati" });
        SeedLabel(new LocalizationLabel() { Key = "User", LanguageId = 2, Value = "Korisnik" });
        SeedLabel(new LocalizationLabel() { Key = "Users", LanguageId = 2, Value = "Korisnici" });
        SeedLabel(new LocalizationLabel() { Key = "Add", LanguageId = 2, Value = "Dodaj" });
        SeedLabel(new LocalizationLabel() { Key = "Edit", LanguageId = 2, Value = "Uredi" });
        SeedLabel(new LocalizationLabel() { Key = "Save", LanguageId = 2, Value = "Sačuvaj" });
        SeedLabel(new LocalizationLabel() { Key = "DateTime", LanguageId = 2, Value = "Datum i vreme" });
        SeedLabel(new LocalizationLabel() { Key = "ID", LanguageId = 2, Value = "#" });
        SeedLabel(new LocalizationLabel() { Key = "Email", LanguageId = 2, Value = "Email" });
        SeedLabel(new LocalizationLabel() { Key = "Password", LanguageId = 2, Value = "Lozinka" });
        SeedLabel(new LocalizationLabel() { Key = "OldPassword", LanguageId = 2, Value = "Stara lozinka" });
        SeedLabel(new LocalizationLabel() { Key = "NewPassword", LanguageId = 2, Value = "Nova lozinka" });
        SeedLabel(new LocalizationLabel() { Key = "Username", LanguageId = 2, Value = "Korisničko ime" });
        SeedLabel(new LocalizationLabel() { Key = "Phone", LanguageId = 2, Value = "Telefon" });
        SeedLabel(new LocalizationLabel() { Key = "HomePage", LanguageId = 2, Value = "Početna" });
        SeedLabel(new LocalizationLabel() { Key = "Settings", LanguageId = 2, Value = "Podešavanja" });
        SeedLabel(new LocalizationLabel() { Key = "ResultListEmpty", LanguageId = 2, Value = "Lista rezultata je prazna" });
        SeedLabel(new LocalizationLabel() { Key = "StartDate", LanguageId = 2, Value = "Početak" });
        SeedLabel(new LocalizationLabel() { Key = "EndDate", LanguageId = 2, Value = "Završetak" });
        SeedLabel(new LocalizationLabel() { Key = "Created", LanguageId = 2, Value = "Kreiran" });
        SeedLabel(new LocalizationLabel() { Key = "CreatedBy", LanguageId = 2, Value = "Kreirao" });
        SeedLabel(new LocalizationLabel() { Key = "LastModified", LanguageId = 2, Value = "Poslednja izmena" });
        SeedLabel(new LocalizationLabel() { Key = "LastModifiedBy", LanguageId = 2, Value = "Poslednji izmenio" });
        SeedLabel(new LocalizationLabel() { Key = "Role", LanguageId = 2, Value = "Rola" });
        SeedLabel(new LocalizationLabel() { Key = "AreYouSure", LanguageId = 2, Value = "Da li ste sigurni?" });
        SeedLabel(new LocalizationLabel() { Key = "Admin", LanguageId = 2, Value = "Admin" });
        SeedLabel(new LocalizationLabel() { Key = "FirstPage", LanguageId = 2, Value = "Prva strana" });
        SeedLabel(new LocalizationLabel() { Key = "LastPage", LanguageId = 2, Value = "Poslednja strana" });
        SeedLabel(new LocalizationLabel() { Key = "Of", LanguageId = 2, Value = "od" });
        SeedLabel(new LocalizationLabel() { Key = "Results", LanguageId = 2, Value = "Rezultati" });
        SeedLabel(new LocalizationLabel() { Key = "Show", LanguageId = 2, Value = "Prikaži" });
        SeedLabel(new LocalizationLabel() { Key = "Hide", LanguageId = 2, Value = "Sakrij" });
        SeedLabel(new LocalizationLabel() { Key = "PageSize", LanguageId = 2, Value = "Veličina stranice" });
        SeedLabel(new LocalizationLabel() { Key = "True", LanguageId = 2, Value = "Da" });
        SeedLabel(new LocalizationLabel() { Key = "False", LanguageId = 2, Value = "Ne" });
        SeedLabel(new LocalizationLabel() { Key = "ActionName", LanguageId = 2, Value = "Naziv akcije" });
        SeedLabel(new LocalizationLabel() { Key = "UnspecifiedError", LanguageId = 2, Value = "Serverska greška" });
        SeedLabel(new LocalizationLabel() { Key = "NotAllowed", LanguageId = 2, Value = "Nedozvoljena akcija" });
        SeedLabel(new LocalizationLabel() { Key = "AlreadyExists", LanguageId = 2, Value = "Duplikat" });
        SeedLabel(new LocalizationLabel() { Key = "Authorization", LanguageId = 2, Value = "Neautorizovan pristup" });
        SeedLabel(new LocalizationLabel() { Key = "TokenParse", LanguageId = 2, Value = "Neispravan token" });
        SeedLabel(new LocalizationLabel() { Key = "CantFind", LanguageId = 2, Value = "Ne postoji" });
        SeedLabel(new LocalizationLabel() { Key = "DatabaseGet", LanguageId = 2, Value = "Problem sa čitanjem iz baze" });
        SeedLabel(new LocalizationLabel() { Key = "DatabaseAdd", LanguageId = 2, Value = "Problem sa dodavanjem u bazu" });
        SeedLabel(new LocalizationLabel() { Key = "DatabaseUpdate", LanguageId = 2, Value = "Problem sa ažuriranjem u bazi" });
        SeedLabel(new LocalizationLabel() { Key = "Filters", LanguageId = 2, Value = "Filteri" });
        SeedLabel(new LocalizationLabel() { Key = "Search", LanguageId = 2, Value = "Pretraži" });
        SeedLabel(new LocalizationLabel() { Key = "CreatedFrom", LanguageId = 2, Value = "Kreiran od" });
        SeedLabel(new LocalizationLabel() { Key = "CreatedTo", LanguageId = 2, Value = "Kreiran do" });
        SeedLabel(new LocalizationLabel() { Key = "StartDateFrom", LanguageId = 2, Value = "Datum početka od" });
        SeedLabel(new LocalizationLabel() { Key = "StartDateTo", LanguageId = 2, Value = "Datum početka do" });
        SeedLabel(new LocalizationLabel() { Key = "EndDateFrom", LanguageId = 2, Value = "Datum završetka od" });
        SeedLabel(new LocalizationLabel() { Key = "EndDateTo", LanguageId = 2, Value = "Datum završetka do" });
        SeedLabel(new LocalizationLabel() { Key = "ResetFilters", LanguageId = 2, Value = "Reset filtera" });
        SeedLabel(new LocalizationLabel() { Key = "Reset", LanguageId = 2, Value = "Resetuj rezultate" });
        #endregion

        #region Client field validation errors
        SeedLabel(new LocalizationLabel() { Key = "RequiredField", LanguageId = 2, Value = "Obavezno polje" });
        SeedLabel(new LocalizationLabel() { Key = "InvalidData", LanguageId = 2, Value = "Neispravan podatak" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan12Chars", LanguageId = 2, Value = "Maksimalan broj karaktera: 12" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan24Chars", LanguageId = 2, Value = "Maksimalan broj karaktera: 24" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan50Chars", LanguageId = 2, Value = "Maksimalan broj karaktera: 50" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan96Chars", LanguageId = 2, Value = "Maksimalan broj karaktera: 96" });
        SeedLabel(new LocalizationLabel() { Key = "NotLessThan8Chars", LanguageId = 2, Value = "Minimalan broj karaktera: 8" });
        SeedLabel(new LocalizationLabel() { Key = "NotLessThan2Chars", LanguageId = 2, Value = "Minimalan broj karaktera: 2" });
        SeedLabel(new LocalizationLabel() { Key = "ChooseAtLeastOne", LanguageId = 2, Value = "Izabrati bar jednu opciju" });
        SeedLabel(new LocalizationLabel() { Key = "MustBeEmail", LanguageId = 2, Value = "Mora biti validan email" });
        #endregion

        #region Server errors
        SeedLabel(new LocalizationLabel() { Key = "ErrorValidationMiddleware", LanguageId = 2, Value = "Problem sa validacijom prosleđenih podataka" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseJwtMalformed", LanguageId = 2, Value = "JWT problem: neispravan format" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseUserId", LanguageId = 2, Value = "JWT problem: nedostaje korisnički ID" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseRole", LanguageId = 2, Value = "JWT problem: nedostaje korisnička rola" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseEmail", LanguageId = 2, Value = "JWT problem: nedostaje korisnički email" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorCantFindKey", LanguageId = 2, Value = "Ne postoji ključ za traženi element" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorCantFindUser", LanguageId = 2, Value = "Ne postoji korisnik" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorCantFindNotification", LanguageId = 2, Value = "Ne postoji notifikacija" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationUser", LanguageId = 2, Value = "Korisnik nije autorizovan" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationUserInactive", LanguageId = 2, Value = "Korisnik nije aktiviran" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationUnconfirmedEmail", LanguageId = 2, Value = "Email nije potvrđen" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationRole", LanguageId = 2, Value = "Neispravna korisnička rola" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationCantResolveUser", LanguageId = 2, Value = "Korisnik nije prepoznat" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationLogin", LanguageId = 2, Value = "Neispravna lozinka" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationMissingClaim", LanguageId = 2, Value = "Claim iz tokena: " });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationMissingToken", LanguageId = 2, Value = "Token nije pronađen" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationTokenExpired", LanguageId = 2, Value = "Sesija istekla" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAlreadyExistsUser", LanguageId = 2, Value = "Duplikat: korisnik već postoji" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorNotAllowedRole", LanguageId = 2, Value = "Nedozvoljena akcija: korisnička rola ne zadovoljava uslove akcije" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorsRequiredPageNumber", LanguageId = 2, Value = "Obavezan podatak: broj stranice" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredPageSize", LanguageId = 2, Value = "Obavezan podatak: veličina stranice" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredAuthToken", LanguageId = 2, Value = "Obavezan podatak: autorizacioni token" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredId", LanguageId = 2, Value = "Obavezan podatak: ID" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredName", LanguageId = 2, Value = "Obavezan podatak: naziv" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredActive", LanguageId = 2, Value = "Obavezan podatak: aktivan" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredPhone", LanguageId = 2, Value = "Obavezan podatak: telefon" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredEmail", LanguageId = 2, Value = "Obavezan podatak: email" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredPassword", LanguageId = 2, Value = "Obavezan podatak: lozinka" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredCurrentUser", LanguageId = 2, Value = "Obavezan podatak: autorizovan korisnik" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorsInvalidDataPageNumber", LanguageId = 2, Value = "Neispravan podatak: broj stranice mora biti veći od 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPageSize", LanguageId = 2, Value = "Neispravan podatak: veličina stranice mora biti veća od 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataId", LanguageId = 2, Value = "Neispravan podatak: ID mora biti veći od 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataNameLongerThan12", LanguageId = 2, Value = "Neispravan podatak: naziv ne može biti duži od 12 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataUserNameLongerThan24", LanguageId = 2, Value = "Neispravan podatak: korisničko ime ne može biti duže od 24 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataUser", LanguageId = 2, Value = "Neispravan podatak: identifikator korisnika mora biti veći od 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPhoneLongerThan48", LanguageId = 2, Value = "Neispravan podatak: telefon ne može biti duži od 48 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataEmailFormat", LanguageId = 2, Value = "Neispravan podatak: email format" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataEmailLongerThan96", LanguageId = 2, Value = "Neispravan podatak: email ne može biti duži od 96 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataFilterEmailShorterThan3", LanguageId = 2, Value = "Neispravan podatak: filter za email ne može biti kraći od 3 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataFilterEmailLongerThan96", LanguageId = 2, Value = "Neispravan podatak: filter za email ne može biti duži od 96 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPasswordLongerThan50", LanguageId = 2, Value = "Neispravan podatak: lozinka ne može biti duža od 50 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPasswordShorterThan8", LanguageId = 2, Value = "Neispravan podatak: lozinka ne može biti kraća od 8 karaktera" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPasswordFormat", LanguageId = 2, Value = "Neispravan podatak: format lozinke" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetUser", LanguageId = 2, Value = "Problem sa izvlačenjem iz baze: korisnik" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetUsers", LanguageId = 2, Value = "Problem sa izvlačenjem iz baze: korisnici" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetLocalizationLabels", LanguageId = 2, Value = "Problem sa izvlačenjem iz baze: labele" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetLanguages", LanguageId = 2, Value = "Problem sa izvlačenjem iz baze: jezici" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetNotifications", LanguageId = 2, Value = "Problem sa izvlačenjem iz baze: notifikacije" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseAddUser", LanguageId = 2, Value = "Problem sa upisom u bazu: korisnik" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseAddNotification", LanguageId = 2, Value = "Problem sa upisom u bazu: notifikacija" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseUpdateUser", LanguageId = 2, Value = "Problem sa ažuriranjem u bazi: korisnik" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseUpdateNotification", LanguageId = 2, Value = "Problem sa ažuriranjem u bazi: notifikacija" });
        #endregion

        #region Notification messages
        SeedLabel(new LocalizationLabel() { Key = "MessageNotificationNewUser", LanguageId = 2, Value = "Dobrodošli!" });
        #endregion

        #endregion

        #region en-US

        #region Labels
        SeedLabel(new LocalizationLabel() { Key = "Login", LanguageId = 1, Value = "Login" });
        SeedLabel(new LocalizationLabel() { Key = "Logout", LanguageId = 1, Value = "Logout" });
        SeedLabel(new LocalizationLabel() { Key = "Ok", LanguageId = 1, Value = "Ok" });
        SeedLabel(new LocalizationLabel() { Key = "Choose", LanguageId = 1, Value = "Choose option" });
        SeedLabel(new LocalizationLabel() { Key = "Scoreboard", LanguageId = 1, Value = "Scoreboard" });
        SeedLabel(new LocalizationLabel() { Key = "User", LanguageId = 1, Value = "User" });
        SeedLabel(new LocalizationLabel() { Key = "Users", LanguageId = 1, Value = "Users" });
        SeedLabel(new LocalizationLabel() { Key = "Add", LanguageId = 1, Value = "Add" });
        SeedLabel(new LocalizationLabel() { Key = "Edit", LanguageId = 1, Value = "Edit" });
        SeedLabel(new LocalizationLabel() { Key = "DateTime", LanguageId = 1, Value = "Date and time" });
        SeedLabel(new LocalizationLabel() { Key = "Save", LanguageId = 1, Value = "Save" });
        SeedLabel(new LocalizationLabel() { Key = "ID", LanguageId = 1, Value = "#" });
        SeedLabel(new LocalizationLabel() { Key = "Email", LanguageId = 1, Value = "Email" });
        SeedLabel(new LocalizationLabel() { Key = "Password", LanguageId = 1, Value = "Password" });
        SeedLabel(new LocalizationLabel() { Key = "OldPassword", LanguageId = 1, Value = "Old password" });
        SeedLabel(new LocalizationLabel() { Key = "NewPassword", LanguageId = 1, Value = "New password" });
        SeedLabel(new LocalizationLabel() { Key = "Username", LanguageId = 1, Value = "Username" });
        SeedLabel(new LocalizationLabel() { Key = "Phone", LanguageId = 1, Value = "Phone" });
        SeedLabel(new LocalizationLabel() { Key = "HomePage", LanguageId = 1, Value = "Home" });
        SeedLabel(new LocalizationLabel() { Key = "Settings", LanguageId = 1, Value = "Settings" });
        SeedLabel(new LocalizationLabel() { Key = "ResultListEmpty", LanguageId = 1, Value = "List is empty" });
        SeedLabel(new LocalizationLabel() { Key = "StartDate", LanguageId = 1, Value = "Start" });
        SeedLabel(new LocalizationLabel() { Key = "EndDate", LanguageId = 1, Value = "End" });
        SeedLabel(new LocalizationLabel() { Key = "Created", LanguageId = 1, Value = "Created" });
        SeedLabel(new LocalizationLabel() { Key = "CreatedBy", LanguageId = 1, Value = "Created by" });
        SeedLabel(new LocalizationLabel() { Key = "LastModified", LanguageId = 1, Value = "Last modified" });
        SeedLabel(new LocalizationLabel() { Key = "LastModifiedBy", LanguageId = 1, Value = "Last modified by" });
        SeedLabel(new LocalizationLabel() { Key = "Role", LanguageId = 1, Value = "Role" });
        SeedLabel(new LocalizationLabel() { Key = "AreYouSure", LanguageId = 1, Value = "Are you sure?" });
        SeedLabel(new LocalizationLabel() { Key = "Admin", LanguageId = 1, Value = "Admin" });
        SeedLabel(new LocalizationLabel() { Key = "FirstPage", LanguageId = 1, Value = "First page" });
        SeedLabel(new LocalizationLabel() { Key = "LastPage", LanguageId = 1, Value = "Last page" });
        SeedLabel(new LocalizationLabel() { Key = "Of", LanguageId = 1, Value = "of" });
        SeedLabel(new LocalizationLabel() { Key = "Results", LanguageId = 1, Value = "Results" });
        SeedLabel(new LocalizationLabel() { Key = "Show", LanguageId = 1, Value = "Show" });
        SeedLabel(new LocalizationLabel() { Key = "Hide", LanguageId = 1, Value = "Hide" });
        SeedLabel(new LocalizationLabel() { Key = "PageSize", LanguageId = 1, Value = "Page size" });
        SeedLabel(new LocalizationLabel() { Key = "True", LanguageId = 1, Value = "Yes" });
        SeedLabel(new LocalizationLabel() { Key = "False", LanguageId = 1, Value = "No" });
        SeedLabel(new LocalizationLabel() { Key = "ActionName", LanguageId = 1, Value = "Action" });
        SeedLabel(new LocalizationLabel() { Key = "NotAllowed", LanguageId = 1, Value = "Action not allowed" });
        SeedLabel(new LocalizationLabel() { Key = "AlreadyExists", LanguageId = 1, Value = "Duplicate" });
        SeedLabel(new LocalizationLabel() { Key = "Authorization", LanguageId = 1, Value = "Unauthorized access" });
        SeedLabel(new LocalizationLabel() { Key = "TokenParse", LanguageId = 1, Value = "Token malformed" });
        SeedLabel(new LocalizationLabel() { Key = "CantFind", LanguageId = 1, Value = "Doesn't exist" });
        SeedLabel(new LocalizationLabel() { Key = "DatabaseGet", LanguageId = 1, Value = "Getting from database problem" });
        SeedLabel(new LocalizationLabel() { Key = "DatabaseAdd", LanguageId = 1, Value = "Updating database problem" });
        SeedLabel(new LocalizationLabel() { Key = "DatabaseUpdate", LanguageId = 1, Value = "Adding to database problem" });
        SeedLabel(new LocalizationLabel() { Key = "Filters", LanguageId = 1, Value = "Filters" });
        SeedLabel(new LocalizationLabel() { Key = "Search", LanguageId = 1, Value = "Search" });
        SeedLabel(new LocalizationLabel() { Key = "CreatedFrom", LanguageId = 1, Value = "Created from" });
        SeedLabel(new LocalizationLabel() { Key = "CreatedTo", LanguageId = 1, Value = "Created to" });
        SeedLabel(new LocalizationLabel() { Key = "StartDateFrom", LanguageId = 1, Value = "Start from" });
        SeedLabel(new LocalizationLabel() { Key = "StartDateTo", LanguageId = 1, Value = "Start to" });
        SeedLabel(new LocalizationLabel() { Key = "EndDateFrom", LanguageId = 1, Value = "End from" });
        SeedLabel(new LocalizationLabel() { Key = "EndDateTo", LanguageId = 1, Value = "End to" });
        SeedLabel(new LocalizationLabel() { Key = "ResetFilters", LanguageId = 1, Value = "Reset filters" });
        SeedLabel(new LocalizationLabel() { Key = "Reset", LanguageId = 1, Value = "Reset results" });
        #endregion

        #region Client field validation errors
        SeedLabel(new LocalizationLabel() { Key = "RequiredField", LanguageId = 1, Value = "Required field" });
        SeedLabel(new LocalizationLabel() { Key = "InvalidData", LanguageId = 1, Value = "Invalid data" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan96Chars", LanguageId = 1, Value = "Maximum length: 96" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan50Chars", LanguageId = 1, Value = "Maximum length: 50" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan24Chars", LanguageId = 1, Value = "Maximum length: 24" });
        SeedLabel(new LocalizationLabel() { Key = "NotLongerThan12Chars", LanguageId = 1, Value = "Maximum length: 12" });
        SeedLabel(new LocalizationLabel() { Key = "NotLessThan8Chars", LanguageId = 1, Value = "Minimum length: 8" });
        SeedLabel(new LocalizationLabel() { Key = "NotLessThan2Chars", LanguageId = 1, Value = "Minimum length: 2" });
        SeedLabel(new LocalizationLabel() { Key = "ChooseAtLeastOne", LanguageId = 1, Value = "Choose one option" });
        SeedLabel(new LocalizationLabel() { Key = "MustBeEmail", LanguageId = 1, Value = "Must be a valid email" });
        #endregion

        #region Server errors
        SeedLabel(new LocalizationLabel() { Key = "ErrorValidationMiddleware", LanguageId = 1, Value = "Error occured while validating sent data" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseJwtMalformed", LanguageId = 1, Value = "JWT problem: invalid format" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseUserId", LanguageId = 1, Value = "JWT problem: missing ID" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseRole", LanguageId = 1, Value = "JWT problem: missing role" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorTokenParseEmail", LanguageId = 1, Value = "JWT problem: missing email" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorCantFindKey", LanguageId = 1, Value = "There is no key for requested data" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorCantFindUser", LanguageId = 1, Value = "Missing user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorCantFindNotification", LanguageId = 1, Value = "Missing notification" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationUser", LanguageId = 1, Value = "Unauthorized user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationUserInactive", LanguageId = 1, Value = "Inactive user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationUnconfirmedEmail", LanguageId = 1, Value = "Email not confirmed" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationRole", LanguageId = 1, Value = "Invalid role" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationCantResolveUser", LanguageId = 1, Value = "Can't resolve user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationLogin", LanguageId = 1, Value = "Invalid password" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationMissingClaim", LanguageId = 1, Value = "Missing claim: " });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationMissingToken", LanguageId = 1, Value = "Token not found" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAuthorizationTokenExpired", LanguageId = 1, Value = "Session expired" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorAlreadyExistsUser", LanguageId = 1, Value = "Duplicate: user already exists" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorNotAllowedRole", LanguageId = 1, Value = "Not allowed: missing role permission" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorsRequiredPageNumber", LanguageId = 1, Value = "Required: page numver" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredPageSize", LanguageId = 1, Value = "Required: page size" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredAuthToken", LanguageId = 1, Value = "Required: token" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredId", LanguageId = 1, Value = "Required: ID" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredName", LanguageId = 1, Value = "Required: name" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredActive", LanguageId = 1, Value = "Required: active" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredPhone", LanguageId = 1, Value = "Required: telephone" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredEmail", LanguageId = 1, Value = "Required: email" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredPassword", LanguageId = 1, Value = "Required: password" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorRequiredCurrentUser", LanguageId = 1, Value = "Required: authorized user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorsInvalidDataPageNumber", LanguageId = 1, Value = "Invalid data: page number must be greater than 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPageSize", LanguageId = 1, Value = "Invalid data: page size must be greater than 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataId", LanguageId = 1, Value = "Invalid data: ID must be greater than 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataNameLongerThan12", LanguageId = 1, Value = "Invalid data: name must be less than 13 characters" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataUserNameLongerThan24", LanguageId = 1, Value = "Invalid data: username must be less than 25 characters" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataUser", LanguageId = 1, Value = "Invalid data: user ID must be greater than 0" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPhoneLongerThan48", LanguageId = 1, Value = "Invalid data: telephone must be less than 49 characters" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataEmailFormat", LanguageId = 1, Value = "Invalid data: email format" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataEmailLongerThan96", LanguageId = 1, Value = "Invalid data: email must be less than 97 characters" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataFilterEmailShorterThan3", LanguageId = 1, Value = "Invalid data: email filter must be greater than 2" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataFilterEmailLongerThan96", LanguageId = 1, Value = "Invalid data: email filter must be less than 97 characters" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPasswordLongerThan50", LanguageId = 1, Value = "Invalid data: password must be less than 51 characters" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPasswordShorterThan8", LanguageId = 1, Value = "Invalid data: password must be greater than 7" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorInvalidDataPasswordFormat", LanguageId = 1, Value = "Invalid data: password format" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetUser", LanguageId = 1, Value = "Database GET problem: user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetUsers", LanguageId = 1, Value = "Database GET problem: users" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetLocalizationLabels", LanguageId = 1, Value = "Database GET problem: localization labels" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetLanguages", LanguageId = 1, Value = "Database GET problem: localization labels" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseGetNotifications", LanguageId = 1, Value = "Database GET problem: notifications" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseAddUser", LanguageId = 1, Value = "Database ADD problem: user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseAddNotification", LanguageId = 1, Value = "Database ADD problem: notification" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseUpdateUser", LanguageId = 1, Value = "Database UPDATE problem: user" });
        SeedLabel(new LocalizationLabel() { Key = "ErrorDatabaseUpdateNotification", LanguageId = 1, Value = "Database UPDATE problem: notification" });
        #endregion

        #region Notification messages
        SeedLabel(new LocalizationLabel() { Key = "MessageNotificationNewUser", LanguageId = 1, Value = "Welcome!" });
        #endregion

        #endregion

        void SeedLabel(LocalizationLabel label)
        {
            var dbLabel = _context.LocalizationLabel.FirstOrDefault(x => x.Key == label.Key && x.LanguageId == label.LanguageId);

            if (dbLabel is null)
                _context.LocalizationLabel.Add(label);
            else
                dbLabel.Value = label.Value;
        }

        await _context.SaveChangesAsync();
    }
    #endregion

    #endregion

    #region Auth
    #region Roles
    private async Task SeedRoles()
    {
        var roles = new List<string>() { "User", "Admin" };

        foreach (var role in roles)
            await SeedRoleAsync(role);

        async Task SeedRoleAsync(string role)
        {
            var dbRole = await _roleManager.FindByNameAsync(role);
            if (dbRole is null)
                await _roleManager.CreateAsync(new IdentityRole { Name = role });
        }

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Users
    private async Task SeedUsers()
    {
        #region List of users to seed
        var users = new List<User>
        {
            new()
            {
                UserName = "admin",
                Email = "admin@email.com",
                PhoneNumber = "+381 60 000 0000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Role = Role.Admin,
                Active = true,
                CreatedBy = "system"
            },
            new()
            {
                UserName = "user",
                Email = "user@email.com",
                PhoneNumber = "+381 60 000 0001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Role = Role.User,
                Active = true,
                CreatedBy = "system"
            }
        };
        #endregion

        foreach (var user in users)
        {
            if (user.Email is null)
                continue;

            var userExists = await _userManager.FindByEmailAsync(user.Email);

            if (userExists is not null)
                continue;

            var createResult = await _userManager.CreateAsync(user, "Rpssl@2024");
            if (createResult.Succeeded)
            {
                user.LastModifiedBy = "system";
                await _userManager.AddToRoleAsync(user, user.Role.ToString());
            }
        }

        await _context.SaveChangesAsync();
    }
    #endregion
    #endregion
}