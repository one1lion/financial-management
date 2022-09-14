using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace FinanMan.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayText<TEnum>(this TEnum enumMem, bool shortVersion = false) where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        var memInfos = enumType.GetMember(enumMem.ToString());
        var dispAttr = (DisplayAttribute?)memInfos
            .FirstOrDefault(mi => mi.DeclaringType == enumType)?
            .GetCustomAttributes(typeof(DisplayAttribute), false)?
            .FirstOrDefault();

        var retVal = enumMem.ToString();

        if (dispAttr is null) { return retVal; }

        retVal = (shortVersion && !string.IsNullOrWhiteSpace(dispAttr.ShortName) ? dispAttr.ShortName : dispAttr.Name) ?? retVal;

        return dispAttr.GetLocalizedString(shortVersion) ?? retVal;
    }

    public static string? GetLocalizedString(this DisplayAttribute dispAttr, bool shortVersion = false)
    {
        if (shortVersion && dispAttr.ShortName is null || !shortVersion && dispAttr.Name is null) { return null; }
        var retVal = shortVersion ? dispAttr.ShortName : dispAttr.Name;
        if (dispAttr.ResourceType is null || string.IsNullOrWhiteSpace(retVal)) { return retVal; }

        var rm = new ResourceManager(dispAttr.ResourceType);

        return rm.GetString(retVal);
    }
}