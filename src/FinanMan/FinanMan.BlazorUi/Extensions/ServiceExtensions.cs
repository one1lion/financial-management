using FinanMan.Abstractions.StateInterfaces;
using FinanMan.BlazorUi.State;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.BlazorUi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddStateManagement(this IServiceCollection services) =>
            services.AddScoped<ILookupListState, LookupListState>();
    }
}
