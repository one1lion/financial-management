namespace FinanMan.BlazorUi.Components.LogoComponents;

public partial class Logo
{
    [Parameter] public LogoType LogoType { get; set; } = LogoType.Svg;
    [Parameter] public string Size { get; set; } = "6rem";
    [Parameter] public string? ShadowColor { get; set; }
    [Parameter] public bool ShowShadow { get; set; } = true;
    [Parameter] public string? TextColor { get; set; }
    [Parameter] public TextPosition TextPosition { get; set; } = TextPosition.Below;
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object?>? AdditionalAttributes { get; set; }

    private readonly string _id = Guid.NewGuid().ToString().Split('-').First();

    private const string _imageUrlPattern = "_content/FinanMan.BlazorUi/imgs/{0}.png";
    private string ImageSrc => string.Format(_imageUrlPattern, ShowShadow ? "ARlogo_NoText_md" : "ARlogo_NoText_NoShadow");

    private string Styles => $"{(string.IsNullOrWhiteSpace(Size) ? null : $"--o1l-size:{Size};")}{(string.IsNullOrWhiteSpace(ShadowColor) ? null : $" --o1l-shadow-color:{ShadowColor};")}{(string.IsNullOrWhiteSpace(TextColor) ? null : $" --o1l-text-color:{TextColor};")}";

    private ElementReference? _logoSvg;
}
