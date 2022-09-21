using FinanMan.Shared.Enums;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

/// <summary>
/// The view model that holds deposit information
/// </summary>
public class DepositEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Deposit;

    [Required]
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }

    public int? AccountId { get; set; }
    [Required]
    public string? TargetAccountValueText {
        get => AccountId?.ToString(); 
        set
        {
            AccountId = int.TryParse(value ?? string.Empty, out var taid) ? taid : default;
        }
    }
    [Required]
    public string? DepositReasonValueText { get; set; }
    public string? Memo { get; set; }
    [Required]
    public double? Amount { get; set; }

    [JsonIgnore]
    public int? TargetAccountId => int.TryParse(TargetAccountValueText ?? string.Empty, out var taid) ? taid : default;
    [JsonIgnore]
    public int? DepositReasonId => int.TryParse(DepositReasonValueText ?? string.Empty, out var drid) ? drid : default;
}

public abstract class TransactionViewModelValidator<TDataEntryViewModel> : AbstractValidator<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{ }    

public class DepositEntryViewModelValidator : TransactionViewModelValidator<DepositEntryViewModel>
{
    public DepositEntryViewModelValidator()
    {
        When(x => x.PostedDate is not null, () =>
        {
            RuleFor(x => x.PostedDate)
                .NotNull()
                .GreaterThanOrEqualTo(x => x.TransactionDate)
                .WithMessage("The posted date must be on or after the Transaction Date.");
        });
    }
}
