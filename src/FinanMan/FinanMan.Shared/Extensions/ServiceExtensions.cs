using FinanMan.Shared.DataEntryModels;
using FinanMan.SharedLocalization;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace FinanMan.Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddFinanManLocalization(this IServiceCollection services) => 
        services
            .AddLocalization(option => option.ResourcesPath = "Resources/Localization")
            .AddTransient<IStringLocalizer, SharedLocalizerService>();

    public static IServiceCollection AddFluentValidation(this IServiceCollection services) =>
        services
            .AddTransient<TransactionViewModelValidator<DepositEntryViewModel>, DepositEntryViewModelValidator>();
}
