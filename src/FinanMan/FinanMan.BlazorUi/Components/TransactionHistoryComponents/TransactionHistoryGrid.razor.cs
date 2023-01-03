using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Extensions;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;

public partial class TransactionHistoryGrid
{
    [Inject] private ITransactionEntryService<DepositEntryViewModel> DepositTransactionService { get; set; } = default!;
    [Inject] private ITransactionEntryService<PaymentEntryViewModel> PaymentTransactionService { get; set; } = default!;
    [Inject] private ITransactionEntryService<TransferEntryViewModel> TransferTransactionService { get; set; } = default!;
    [Inject] private IStringLocalizer<TransactionHistoryGrid> Localizer { get; set; } = default!;

    private List<Transaction>? _transactions;
    private IEnumerable<Transaction>? SortedTransactions => GetSortedTransactions();

    /// <summary>
    /// Tracks the list of columns being sorted (Keys) and whether they are 
    /// sorted descending or not (Values).
    /// </summary>
    private readonly List<SortColumn> _sortColumns = new();

    protected override Task OnInitializedAsync()
    {
        return RefreshTransactions();
    }

    private async Task RefreshTransactions()
    {
        var asOfDate = (_transactions?.Any() ?? false)
            ? _transactions.Max(t => t.TransactionDate)
            : default(DateTime?);

        var depTransTask = DepositTransactionService.GetTransactionsAsync(asOfDate: asOfDate);
        var paymentTransTask = PaymentTransactionService.GetTransactionsAsync(asOfDate: asOfDate);
        var transferTransTask = TransferTransactionService.GetTransactionsAsync(asOfDate: asOfDate);

        await Task.WhenAll(depTransTask, paymentTransTask, transferTransTask);

        var depResp = depTransTask.Result;
        var payResp = paymentTransTask.Result;
        var traResp = transferTransTask.Result;

        _transactions?.Clear();

        if (depResp?.WasError ?? true)
        {
            // TODO: Do something special with the error
        }
        else if (depResp.Data is not null)
        {
            AddTransactionsToList(depResp.Data.ToEntityModel().ToList());
        }
        
        if (payResp?.WasError ?? true)
        {
            // TODO: Do something special with the error
        }
        else if (payResp.Data is not null)
        {
            AddTransactionsToList(payResp.Data.ToEntityModel().ToList());
        }
        
        if (traResp?.WasError ?? true)
        {
            // TODO: Do something special with the error
        }
        else if (traResp.Data is not null)
        {
            AddTransactionsToList(traResp.Data.ToEntityModel().ToList());
        }
    }

    private void AddTransactionsToList(IEnumerable<Transaction> transactions)
    {
        if (_transactions is null)
        {
            _transactions = transactions.ToList();
        }
        else
        {
            _transactions.AddRange(transactions);
        }
    }

    private Task HandleColumnHeaderClicked(ColumnName columnName)
    {
        if (_sortColumns.Any(x => x.Column != columnName)) { _sortColumns.Clear(); }

        var sortCol = _sortColumns.FirstOrDefault(x => x.Column == columnName);
        if (sortCol is null)
        {
            //_sortColumns.Insert(0, new SortColumn() { Column = columnName, Descending = false });
            _sortColumns.Add(new SortColumn() { Column = columnName, Descending = false });
        }
        else if (!sortCol.Descending)
        {
            sortCol.Descending = !sortCol.Descending;
        }
        else
        {
            _sortColumns.Remove(sortCol);
        }
        return InvokeAsync(StateHasChanged);
    }

    private Task SortByColumn(ColumnName columnName, SortDir dir)
    {
        if (_sortColumns.Any(x => x.Column != columnName)) { _sortColumns.Clear(); }

        var sortCol = _sortColumns.FirstOrDefault(x => x.Column == columnName);
        var alreadyApplied =
            // The column is not sorted and we are trying to sort to none
            (sortCol is null && dir == SortDir.None)
            // Or the column is already sorted by the request direction
            || (sortCol is not null
                && ((sortCol.Descending && dir == SortDir.Desc)
                    || (!sortCol.Descending && dir == SortDir.Asc)));
        if (alreadyApplied)
        {
            return Task.CompletedTask;
        }

        if (sortCol is null)
        {
            sortCol = new SortColumn() { Column = columnName, Descending = dir == SortDir.Desc };
            _sortColumns.Add(sortCol);
            return InvokeAsync(StateHasChanged);
        }
        else if (dir == SortDir.None)
        {
            _sortColumns.Remove(sortCol);
            return InvokeAsync(StateHasChanged);
        }

        sortCol.Descending = dir == SortDir.Desc;

        return InvokeAsync(StateHasChanged);
    }

    private SortDir GetSortedDir(ColumnName curCol) =>
        _sortColumns.FirstOrDefault(x => x.Column == curCol)?.Descending switch
        {
            null => SortDir.None,
            true => SortDir.Desc,
            false => SortDir.Asc
        };

    private IEnumerable<Transaction>? GetSortedTransactions()
    {
        if (_transactions is null) { return default; }

        if (!_sortColumns.Any()) { return _transactions.OrderByDescending(x => x.TransactionDate); }

        var sortedTrans = _transactions.OrderBy(x => 1);

        foreach (var curSort in _sortColumns)
        {
            Func<Transaction, object?> sortColProp = default!;

            switch (curSort.Column)
            {
                case ColumnName.PendingColumn:
                    sortColProp = x => x.PostingDate ?? DateTime.MinValue;
                    break;
                case ColumnName.TransDateColumn:
                    sortColProp = x => x.TransactionDate;
                    break;
                case ColumnName.AccountColumn:
                    sortColProp = x => x.Account.Name;
                    break;
                case ColumnName.PayeeColumn:
                    sortColProp = x =>
                        x.TransactionType switch
                        {
                            TransactionType.Payment => x.Payment?.Payee?.Name,
                            TransactionType.Transfer => x.Transfer?.TargetAccount?.Name,
                            _ => default
                        };
                    break;
                case ColumnName.MemoColumn:
                    sortColProp = x => x.Memo;
                    break;
                case ColumnName.TotalColumn:
                    sortColProp = x =>
                    (x.TransactionType switch
                    {
                        TransactionType.Payment => x.Payment?.PaymentDetails.Sum(y => y.Amount),
                        TransactionType.Deposit => x.Deposit?.Amount,
                        TransactionType.Transfer => x.Transfer?.Amount,
                        _ => default
                    }) ?? 0;
                    break;
            }

            if (curSort.Descending)
            {
                sortedTrans = sortedTrans.ThenByDescending(sortColProp);
            }
            else
            {
                sortedTrans = sortedTrans.ThenBy(sortColProp);
            }
        }

        _transactions = sortedTrans.ToList();
        return sortedTrans;
    }
}

public class SortColumn
{
    public ColumnName Column { get; init; }
    public bool Descending { get; set; }
}

public enum ColumnName
{
    [Display(Name = "Pending")]
    PendingColumn,
    [Display(Name = "Trans Date")]
    TransDateColumn,
    [Display(Name = "Account")]
    AccountColumn,
    [Display(Name = "Payee")]
    PayeeColumn,
    [Display(Name = "Memo")]
    MemoColumn,
    [Display(Name = "Amount")]
    TotalColumn
}

public enum SortDir
{
    None,
    Asc,
    Desc
}