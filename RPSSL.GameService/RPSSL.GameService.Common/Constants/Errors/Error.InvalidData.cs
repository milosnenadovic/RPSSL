namespace RPSSL.GameService.Common.Constants.Errors;

public static partial class Error
{
	public static class InvalidData
	{
		public static string PageNumber => "ErrorsInvalidDataPageNumber";
		public static string PageSize => "ErrorInvalidDataPageSize";
		public static string Id => "ErrorInvalidDataId";
		public static string NameLongerThan12 => "ErrorInvalidDataNameLongerThan12";
		public static string UserNameLongerThan24 => "ErrorInvalidDataUserNameLongerThan24";
		public static string User => "ErrorInvalidDataUser";
		public static string PhoneLongerThan48 => "ErrorInvalidDataPhoneLongerThan48";
		public static string EmailFormat => "ErrorInvalidDataEmailFormat";
		public static string EmailLongerThan96 => "ErrorInvalidDataEmailLongerThan96";
		public static string FilterEmailShorterThan3 => "ErrorInvalidDataFilterEmailShorterThan3";
		public static string FilterEmailLongerThan96 => "ErrorInvalidDataFilterEmailLongerThan96";
		public static string PasswordLongerThan50 => "ErrorInvalidDataPasswordLongerThan50";
		public static string PasswordShorterThan8 => "ErrorInvalidDataPasswordShorterThan8";
		public static string PasswordFormat => "ErrorInvalidDataPasswordFormat";
		public static string PlayerChoice => "ErrorInvalidDataPlayerChoice";
		public static string ComputerChoice => "ErrorInvalidDataComputerChoice";
		public static string Language => "ErrorInvalidDataLanguage";
	}
}
