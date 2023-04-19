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
            .AddLocalization(option => option.ResourcesPath = "Resources/Localization");

    public static IServiceCollection AddFluentValidation(this IServiceCollection services) =>
        services
            .AddSingleton<TransactionViewModelValidator<DepositEntryViewModel>, DepositEntryViewModelValidator>()
            .AddSingleton<TransactionViewModelValidator<PaymentEntryViewModel>, PaymentEntryViewModelValidator>()
            .AddSingleton<TransactionViewModelValidator<TransferEntryViewModel>, TransferEntryViewModelValidator>();
}
