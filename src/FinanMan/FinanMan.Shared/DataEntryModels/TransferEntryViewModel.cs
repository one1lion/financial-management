﻿using FinanMan.Shared.Enums;

namespace FinanMan.Shared.DataEntryModels;

public class TransferEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Transfer;
    
    private string? _sourceAccountValueText;
    public string? SourceAccountValueText
    {
        get => _sourceAccountValueText;
        set
        {
            if (_sourceAccountValueText == value) { return; }
            _sourceAccountValueText = value;
            if(TargetAccountValueText == value) { TargetAccountValueText = null; }
        }
    }
    public string? TargetAccountValueText { get; set; }
    public string? Memo { get; set; }
    public double? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
}
