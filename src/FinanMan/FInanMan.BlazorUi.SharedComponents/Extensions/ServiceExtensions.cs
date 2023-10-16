using FinanMan.BlazorUi.SharedComponents.JsInterop;
using Microsoft.Extensions.DependencyInjection;

namespace FinanMan.BlazorUi.SharedComponents.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddJavaScriptModules(this IServiceCollection services) 
        => services.AddTransient<IMyIsolatedModule, MyIsolatedModule>();
}
