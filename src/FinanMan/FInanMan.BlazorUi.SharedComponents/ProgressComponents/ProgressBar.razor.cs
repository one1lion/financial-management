using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.ProgressComponents;
public partial class ProgressBar
{
    [Parameter] public int Value { get; set; } = -1;
    [Parameter] public string? Width { get; set; }
    [Parameter] public string? Height { get; set; }
    [Parameter] public string? Thickness { get; set; }
    [Parameter] public bool AnimateInitialFill { get; set; }
    [Parameter] public double InfiniteAnimationTimeInSeconds { get; set; } = 1.4;
    [Parameter] public ProgressLabelPosition ProgressLabelPosition { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? AdditionalCssClass { get; set; }

    private string CssClasses => $"{(Value < 0 ? "infinite" : string.Empty)}{(Completed ? " complete" : string.Empty)}{(AnimateInitialFill ? " init-fill" : string.Empty)}{(!string.IsNullOrWhiteSpace(AdditionalCssClass) ? $" {AdditionalCssClass}" : string.Empty)}".Trim();

    private string LabelCssClass => ProgressLabelPosition switch
    {
        ProgressLabelPosition.Start => "start",
        ProgressLabelPosition.Center => "center",
        ProgressLabelPosition.End => "end",
        _ => string.Empty
    };
    private bool Completed => Value == 100;
    public string Style => $"--value:{Value};--animDuration:{InfiniteAnimationTimeInSeconds}s;{(string.IsNullOrWhiteSpace(Width) ? string.Empty : $"--width: {Width};")}{(string.IsNullOrWhiteSpace(Height) ? string.Empty : $"--height: {Height};")}{(string.IsNullOrWhiteSpace(Thickness) ? string.Empty : $"--thickness: {Thickness}")}";
}
