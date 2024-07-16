using RPSSL.Web.Domain.Models;

namespace RPSSL.Web.Helpers;

public class LocalizationManager
{
    private static LocalizedData? LocalizedData { get; set; }

    public static bool IsInitialized
    {
        get
        {
            return LocalizedData != null;
        }
    }

    public static void Initialize(LocalizedData localizedData)
    {
        if (LocalizedData is null)
            LocalizedData = localizedData;
    }

    public string this[string key] => Get(key);

    public string Get(string key)
    {
        return Get(key, System.Globalization.CultureInfo.CurrentCulture.Name);
    }

    public string Get(string key, string culture)
    {
        try
        {
            var locItemTranslation = LocalizedData?.LocalizationLabels.FirstOrDefault(x => x.Key == key);
            if (locItemTranslation is null)
                return "[" + key + "]";
            else
                return locItemTranslation.Value;
        }
        catch
        {
            return key;
        }
    }

    public static string StaticGet(string key)
    {
        return StaticGet(key, System.Globalization.CultureInfo.CurrentCulture.Name);
    }

    public static string StaticGet(string key, string culture)
    {
        try
        {
            var locItemTranslation = LocalizedData?.LocalizationLabels.FirstOrDefault(x => x.Key == key);
            if (locItemTranslation is null)
                return "[" + key + "]";
            else
                return locItemTranslation.Value;
        }
        catch
        {
            return key;
        }
    }

    public static void RefreshCache(LocalizedData localizedData)
    {
        LocalizedData = localizedData;
    }
}
