namespace FinanMan.BlazorUi.Shared;

public partial class Footer
{
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object?>? AdditionalAttributes { get; set; }
}
