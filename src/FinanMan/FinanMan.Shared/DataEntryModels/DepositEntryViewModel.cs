﻿using FinanMan.Shared.Enums;

namespace FinanMan.Shared.DataEntryModels;

/// <summary>
/// The view model that holds deposit information
/// </summary>
public class DepositEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Deposit;

    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? TargetAccountValueText { get; set; }
    public string? DepositReasonValueText { get; set; }
    public string? Memo { get; set; }
    public double? Amount { get; set; }

}
