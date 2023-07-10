using FinanMan.BlazorUi.SharedComponents.IconComponents.Lib;
using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.IconComponents;

public partial class Icon
{
    [Parameter] public IconName IconName { get; set; }

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