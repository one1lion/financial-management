using FinanMan.BlazorUi.SharedComponents.JsInterop;
using Microsoft.Extensions.DependencyInjection;

namespace FinanMan.BlazorUi.SharedComponents.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds the Isolated JavaScript modules to the service collection
    /// </summary>
    /// <param name="services"> 
    /// The service collection to add the modules to
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddJavaScriptModules(this IServiceCollection services) 
        => services.AddTransient<IMyIsolatedModule, MyIsolatedModule>();
}
