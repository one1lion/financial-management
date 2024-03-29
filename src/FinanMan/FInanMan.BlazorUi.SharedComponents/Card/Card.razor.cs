using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.Card;

/// <summary>
/// A shareable Card component that will render html markup
/// for a card given the card content
/// </summary>
/// <remarks>
/// Example Usage:
/// <code>
/// &lt;Card&gt;
///     &lt;CardHeader&gt;
/// &lt;/Card&gt;
/// </code>
/// </remarks>
public partial class Card
{
    /// <summary>
    /// The content to display in the card
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// Additional css classes to apply to the card
    /// </summary>
    [Parameter] public string? AdditionalCssClasses { get; set; }
    /// <summary>
    /// Additional attributes to apply to the card
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? Attributes { get; set; }

    private string CssClasses => $"card{(string.IsNullOrWhiteSpace(AdditionalCssClasses) ? string.Empty : $" {AdditionalCssClasses}")}";
}