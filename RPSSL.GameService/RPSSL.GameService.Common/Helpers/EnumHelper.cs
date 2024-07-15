using System.ComponentModel;
using System.Reflection;

namespace RPSSL.GameService.Common.Helpers;

public static class EnumHelper
{
	#region GetEnumDescription
	/// <summary>
	/// Get Description string from Enums in Core.Enums
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string GetEnumDescription(Enum value)
	{
		FieldInfo? fi = value.GetType().GetField(value.ToString());

		if (fi is null)
			return value.ToString();
		else
		{
			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute),
				false);

			if (attributes != null &&
				attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}
	}
	#endregion
}
