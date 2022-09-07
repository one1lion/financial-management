using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? AdditionalCssClasses { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private string CssClasses => $"card{(string.IsNullOrWhiteSpace(AdditionalCssClasses) ? string.Empty : $" {AdditionalCssClasses}")}";
}