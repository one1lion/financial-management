using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.ProgressComponents;

/// <summary>
/// A shareable Progress Bar component that will render html markup
/// </summary>
public partial class ProgressBar
{
    /// <summary>
    /// The current value of the progress bar
    /// </summary>
    [Parameter] public int Value { get; set; } = -1;
    /// <summary>
    /// The width of the progress bar
    /// </summary>
    [Parameter] public string? Width { get; set; }
    /// <summary>
    /// The height of the progress bar
    /// </summary>
    [Parameter] public string? Height { get; set; }
    /// <summary>
    /// The thickness of the progress bar fill
    /// </summary>
    [Parameter] public string? Thickness { get; set; }
    /// <summary>
    /// Whether or not to animate the initial fill
    /// </summary>
    [Parameter] public bool AnimateInitialFill { get; set; }
    /// <summary>
    /// The time in seconds the infinite animation should take between repeats
    /// </summary>
    [Parameter] public double InfiniteAnimationTimeInSeconds { get; set; } = 1.4;
    /// <summary>
    /// The position of the progress label
    /// </summary>
    [Parameter] public ProgressLabelPosition ProgressLabelPosition { get; set; }
    /// <summary>
    /// The content to display in the progress bar
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// Additional css classes to apply to the progress bar
    /// </summary>
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
    /// <summary>
    /// The calculated style to apply to the progress bar
    /// </summary>
    public string Style => $"--value:{Value};--animDuration:{InfiniteAnimationTimeInSeconds}s;{(string.IsNullOrWhiteSpace(Width) ? string.Empty : $"--width: {Width};")}{(string.IsNullOrWhiteSpace(Height) ? string.Empty : $"--height: {Height};")}{(string.IsNullOrWhiteSpace(Thickness) ? string.Empty : $"--thickness: {Thickness}")}";
}
