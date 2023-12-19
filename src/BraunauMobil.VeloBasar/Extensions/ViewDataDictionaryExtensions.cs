using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BraunauMobil.VeloBasar.Extensions;

public static class ViewDataDictionaryExtensions
{
    private const string _activeBasarKey = "ActiveBasar";

    public static BasarEntity? GetActiveBasar(this ViewDataDictionary viewData)
    {
        ArgumentNullException.ThrowIfNull(viewData);

        return viewData[_activeBasarKey] as BasarEntity;
    }

    public static void SetActiveBasar(this ViewDataDictionary viewData, BasarEntity? basar)
    {
        ArgumentNullException.ThrowIfNull(viewData);

        viewData[_activeBasarKey] = basar;
    }

    private const string _activeSessionIdKey = "ActiveSessionId";

    public static int? GetActiveSessionId(this ViewDataDictionary viewData)
    {
        ArgumentNullException.ThrowIfNull(viewData);

        if (viewData[_activeSessionIdKey] is int sessionId)
        {
            return sessionId;
        }
        
        return null;
    }

    public static void SetActiveSessionId(this ViewDataDictionary viewData, int? sessionId)
    {
        ArgumentNullException.ThrowIfNull(viewData);

        viewData[_activeSessionIdKey] = sessionId;
    }
}
