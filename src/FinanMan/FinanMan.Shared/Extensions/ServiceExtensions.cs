using FinanMan.SharedLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace FinanMan.Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddFinanManLocalization(this IServiceCollection services)
    {
        return services
            .AddLocalization(option => option.ResourcesPath = "Resources/Localization")
            .AddTransient<IStringLocalizer, SharedLocalizerService>();
    }
}
