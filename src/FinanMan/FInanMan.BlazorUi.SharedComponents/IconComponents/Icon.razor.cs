using FinanMan.BlazorUi.SharedComponents.IconComponents.Lib;
using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.IconComponents;

/// <summary>
/// A shareable Icon component that will render html markup
/// </summary>
public partial class Icon
{
    /// <summary>
    /// The <see cref="IconName" /> of the icon to render
    /// </summary>
    [Parameter] public IconName IconName { get; set; }

    /// <summary>
    /// Additional attributes to apply to the icon
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    private RenderFragment Render => builder =>
    {
        var curElem = 0;
        var curType = IconName.GetIconType();
        if (curType is not null)
        {
            builder.OpenComponent(curElem++, curType);
            builder.CloseComponent();
        }
    };
}