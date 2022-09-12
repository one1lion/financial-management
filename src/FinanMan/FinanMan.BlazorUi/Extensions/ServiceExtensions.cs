using FinanMan.BlazorUi.State;
using FinanMan.Shared.StateInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinanMan.BlazorUi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddStateManagement(this IServiceCollection services) =>
            services.AddScoped<ILookupListState, LookupListState>();
    }
}
