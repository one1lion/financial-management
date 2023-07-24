using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanMan.SharedClient.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services) =>
        services
            .AddTransient<IAccountService, AccountService>()
            .AddTransient<ILookupListService, LookupItemService>()
            .AddTransient(typeof(ITransactionEntryService<>), typeof(TransactionEntryService<>));
}
