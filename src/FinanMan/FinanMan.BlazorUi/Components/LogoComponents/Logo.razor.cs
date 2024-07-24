namespace FinanMan.BlazorUi.Components.LogoComponents;

public partial class Logo
{
    [Parameter] public LogoType LogoType { get; set; } = LogoType.Svg;
    [Parameter] public string Size { get; set; } = "6rem";
    [Parameter] public string ShaddowColor { get; set; } = "rgba(44, 44, 44, .2)";
    [Parameter] public bool ShowShadow { get; set; } = true;

    private readonly string _id = Guid.NewGuid().ToString().Split('-').First();

    private const string _imageUrlPattern = "_content/FinanMan.BlazorUi/imgs/{0}.png";
    private string ImageSrc => string.Format(_imageUrlPattern, ShowShadow ? "ARlogo_NoText_md" : "ARlogo_NoText_NoShadow");

    private ElementReference? _logoSvg;

}
