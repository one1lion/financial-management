namespace FinanMan.BlazorUi.Components.LogoComponents;

public partial class Logo
{
    [Parameter] public LogoType LogoType { get; set; } = LogoType.Svg;
    [Parameter] public string Size { get; set; } = "6rem";
    [Parameter] public string ShadowColor { get; set; } = "rgba(44, 44, 44, .2)";
    [Parameter] public bool ShowShadow { get; set; } = true;

    private readonly string _id = Guid.NewGuid().ToString().Split('-').First();

    private const string _imageUrlPattern = "_content/FinanMan.BlazorUi/imgs/{0}.png";
    private string ImageSrc => string.Format(_imageUrlPattern, ShowShadow ? "ARlogo_NoText_md" : "ARlogo_NoText_NoShadow");

    private string Styles => $"{(string.IsNullOrWhiteSpace(Size) ? null : $"--size:{Size}")};{(string.IsNullOrWhiteSpace(ShadowColor) ? null : $" --color2:{ShadowColor}")}";

    private ElementReference? _logoSvg;

}
