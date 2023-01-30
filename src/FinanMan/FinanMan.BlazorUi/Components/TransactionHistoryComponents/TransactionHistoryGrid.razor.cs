using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Extensions;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;

public partial class TransactionHistoryGrid
{
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;

    private readonly string _id = Guid.NewGuid().ToString();

    private List<ITransactionDataEntryViewModel>? _transactions => TransactionsState.Transactions;
    private IEnumerable<ITransactionDataEntryViewModel>? SortedTransactions => GetSortedTransactions();

    /// <summary>
    /// Tracks the list of columns being sorted (Keys) and whether they are 
    /// sorted descending or not (Values).
    /// </summary>
    private readonly List<SortColumn> _sortColumns = new();
    private bool _showPostedDateEntry;
    private ITransactionDataEntryViewModel? _transactionToEdit;
    protected override Task OnInitializedAsync()
    {
        TransactionsState.OnTransactionHistoryChanged += HandleTransactionHistoryChanged;
        return TransactionsState.RefreshTransactionHistoryAsync();
    }

    private Task HandleTransactionHistoryChanged()
    {
        return InvokeAsync(StateHasChanged);
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

    private IEnumerable<ITransactionDataEntryViewModel>? GetSortedTransactions()
    {
        if (_transactions is null) { return default; }

        if (!_sortColumns.Any()) { return _transactions.OrderByDescending(x => x.TransactionDate); }

        var sortedTrans = _transactions.OrderBy(x => 1);

        foreach (var curSort in _sortColumns)
        {
            Func<ITransactionDataEntryViewModel, object?> sortColProp = default!;

            switch (curSort.Column)
            {
                case ColumnName.PostedDateColumn:
                    sortColProp = x => x.PostedDate ?? DateTime.MinValue;
                    break;
                case ColumnName.TransDateColumn:
                    sortColProp = x => x.TransactionDate;
                    break;
                case ColumnName.AccountColumn:
                    sortColProp = x => x.AccountName;
                    break;
                case ColumnName.PayeeColumn:
                    sortColProp = x =>
                        x switch
                        {
                            PaymentEntryViewModel p => p.PayeeName,
                            TransferEntryViewModel t => t.TargetAccountName,
                            _ => default
                        };
                    break;
                case ColumnName.MemoColumn:
                    sortColProp = x => x.Memo;
                    break;
                case ColumnName.TotalColumn:
                    sortColProp = x => x.Total;
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

        // TODO: Decide whether to apply the current sort to the underlying transactions history list from state
        TransactionsState.Transactions = sortedTrans.ToList();
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
    [Display(Name = "Posted Date")]
    PostedDateColumn,
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