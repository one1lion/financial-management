using FinanMan.BlazorUi.SharedComponents.IconComponents.Lib.Icons;

namespace FinanMan.BlazorUi.SharedComponents.IconComponents.Lib;
/// <summary>
/// Named icons used within the application
/// </summary>
public enum IconName
{
    /// <summary>
    /// The add icon
    /// </summary>
    [IconInfo(IconType = typeof(Add))]
    Add,
    /// <summary>
    /// The check mark icon
    /// </summary>
    [IconInfo(IconType = typeof(CheckMark))]
    CheckMark,
    /// <summary>
    /// The trash can icon
    /// </summary>
    [IconInfo(IconType = typeof(TrashCan))]
    TrashCan
}

/// <summary>
/// Extensions for the <see cref="IconName"/> enum
/// </summary>
public static class IconNameExtensions
{
    /// <summary>
    /// Gets the icon type for the given icon name
    /// </summary>
    /// <param name="iconName">
    /// The <see cref="IconName" /> to get the type for
    /// </param>
    /// <returns>
    /// The <see cref="Type" /> of the icon
    /// </returns>
    /// <remarks>
    /// The icon <see cref="Type"/> returned will be one of the 
    /// Blazor component types in the <see cref="Icon"/> 
    /// namespace
    /// </remarks>
    public static Type? GetIconType(this IconName iconName)
    {
        var iconInfo = iconName.GetIconInfo();
        return iconInfo?.IconType;
    }

    /// <summary>
    /// Gets the <see cref="IconInfoAttribute"/> for the given icon name
    /// </summary>
    /// <param name="iconName">
    /// The <see cref="IconName" /> to get the <see cref="IconInfoAttribute"/> for
    /// </param>
    /// <returns>
    /// The <see cref="IconInfoAttribute"/> for the given icon name
    /// </returns>
    public static IconInfoAttribute? GetIconInfo(this IconName iconName)
    {
        var iconInfo = iconName.GetType().GetField(iconName.ToString())?.GetCustomAttributes(typeof(IconInfoAttribute), false).FirstOrDefault();
        return iconInfo as IconInfoAttribute;
    }
}
