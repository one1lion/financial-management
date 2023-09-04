using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.SharedComponents.DraggableComponents;

/// <summary>
/// A container that can be dragged around the screen.
/// </summary>
public partial class DraggableContainer
{
    /// <summary>
    /// The content to be displayed in the draggable container.
    /// </summary>
    [Parameter, AllowNull] public RenderFragment ChildContent { get; set; }

    private readonly string _id = Guid.NewGuid().ToString();


}
