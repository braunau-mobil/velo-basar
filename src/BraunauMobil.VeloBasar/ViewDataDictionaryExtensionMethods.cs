using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar;

public static class ViewDataDictionaryExtensionMethods
{
    private const string _titleKey = "Title";

    public static LocalizedString Title(this ViewDataDictionary viewData)
    {
        LocalizedString? title = GetData<LocalizedString>(viewData, _titleKey);
        if (title == null)
        {
            throw new InvalidOperationException("No page title set.");
        }
        return title;

    }

    public static void SetTitle(this ViewDataDictionary viewData, LocalizedString value)
        => SetData(viewData, _titleKey, value);

    private static T? GetData<T>(this ViewDataDictionary viewData, string key)
    {
        ArgumentNullException.ThrowIfNull(viewData);
        ArgumentNullException.ThrowIfNull(key);

        object? data = viewData[key];
        if (data == null)
        {
            return default;
        }
        return (T)data;
    }

    private static void SetData<T>(this ViewDataDictionary viewData, string key, T? data)
    {
        ArgumentNullException.ThrowIfNull(viewData);
        ArgumentNullException.ThrowIfNull(key);
        
        viewData[key] = data ?? throw new ArgumentNullException(nameof(data));
    }
}
