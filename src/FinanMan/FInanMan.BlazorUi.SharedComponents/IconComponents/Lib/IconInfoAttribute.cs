namespace FinanMan.BlazorUi.SharedComponents.IconComponents.Lib;

/// <summary>
/// The attribute that contains the icon type
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class IconInfoAttribute : Attribute
{
    /// <summary>
    /// The icon type
    /// </summary>
    /// <remarks>
    /// This should be one of the Blazor component types 
    /// in the <see cref="IconComponents.Icon"/> namespace
    /// </remarks>
    public required Type IconType { get; set; }
}