using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanMan.SharedClient.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services) =>
        services
            .AddTransient(typeof(IAccountService), typeof(AccountService))
            .AddTransient(typeof(ILookupListService), typeof(LookupItemService))
            .AddTransient(typeof(ITransactionEntryService<>), typeof(TransactionEntryService<>));
}
