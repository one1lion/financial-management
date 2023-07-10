using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanMan.SharedServer.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection SetupDbContext(this IServiceCollection services, IConfiguration config) =>
        services.AddDbContextFactory<FinanManContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DbConnection")));

    public static IServiceCollection AddServerServices(this IServiceCollection services) =>
        services
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<ILookupListService, LookupItemService>()
            .AddScoped(typeof(ITransactionEntryService<>), typeof(TransactionEntryService<>));
}
