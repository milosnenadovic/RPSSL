using System.Text.RegularExpressions;

namespace RPSSL.Web.Helpers;

public static class PasswordValidator
{
    public static readonly string PasswordValidation_Regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    public static readonly Regex PasswordValidation_Regex_Compiled = new(PasswordValidation_Regex);

    public static bool IsValidPassword(string password)
    {
        var isValid = PasswordValidation_Regex_Compiled.IsMatch(password);
        return isValid;
    }
}