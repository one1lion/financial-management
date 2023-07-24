using FinanMan.BlazorUi.SharedComponents.IconComponents.Lib.Icons;

namespace FinanMan.BlazorUi.SharedComponents.IconComponents.Lib;
public enum IconName
{
    [IconInfo(IconType = typeof(Add))]
    Add,
    [IconInfo(IconType = typeof(CheckMark))]
    CheckMark,
    [IconInfo(IconType = typeof(TrashCan))]
    TrashCan
}

public static class IconExtensions
{
    public static Type? GetIconType(this IconName iconName)
    {
        var iconInfo = iconName.GetIconInfo();
        return iconInfo?.IconType;
    }

    public static IconInfoAttribute? GetIconInfo(this IconName iconName)
    {
        var iconInfo = iconName.GetType().GetField(iconName.ToString())?.GetCustomAttributes(typeof(IconInfoAttribute), false).FirstOrDefault();
        return iconInfo as IconInfoAttribute;
    }
}
