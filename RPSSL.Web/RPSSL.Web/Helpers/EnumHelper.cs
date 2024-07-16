using System.ComponentModel;
using System.Reflection;

namespace RPSSL.Web.Helpers;

public static class EnumHelper
{
    #region GetEnumDescription
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

    #region GetValueFromDescription
    public static T GetValueFromDescription<T>(string description)
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();
        foreach (var field in type.GetFields())
        {
            DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute is not null)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field?.GetValue(null);
            }
        }
        throw new ArgumentException("Not found.", nameof(description));
    }
    #endregion
}
