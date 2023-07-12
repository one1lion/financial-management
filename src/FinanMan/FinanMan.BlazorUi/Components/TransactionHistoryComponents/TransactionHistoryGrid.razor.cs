using FinanMan.Shared.DataEntryModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;

public partial class TransactionHistoryGrid : IDisposable
{
    private const int _colCount = 7;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    [Inject] private ILookupListState LookupListState { get; set; } = default!;

    [Parameter] public string? AccountName { get; set; }

    private readonly string _id = Guid.NewGuid().ToString();

    private IEnumerable<ITransactionDataEntryViewModel>? Transactions => TransactionsState.Transactions?.Where(x => string.IsNullOrWhiteSpace(AccountName) || x.AccountName == AccountName);
    private IEnumerable<ITransactionDataEntryViewModel> DisplayedTransactions => GetSortedTransactions();

    /// <summary>
    /// Tracks the list of columns being sorted (Keys) and whether they are 
    /// sorted descending or not (Values).
    /// </summary>
    private readonly List<SortColumn> _sortColumns = new();
    private bool _showPostedDateEntry;

    protected override Task OnInitializedAsync()
    {
        TransactionsState.OnTransactionHistoryChanged += HandleTransactionHistoryChanged;
        LookupListState.PropertyChanged += HandlePropertyChanged;
        return TransactionsState.RefreshTransactionHistoryAsync();
    }

    private async void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(ILookupListState.Initialized) || e.PropertyName == nameof(ILookupListState.LookupItemCache))
        {
            await InvokeAsync(StateHasChanged);
        }   
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

    private IEnumerable<ITransactionDataEntryViewModel> GetSortedTransactions()
    {
        if (Transactions is null) { return Enumerable.Empty<ITransactionDataEntryViewModel>(); }

        if (!_sortColumns.Any()) { return Transactions.OrderByDescending(x => x.TransactionDate); }

        var sortedTrans = Transactions.OrderBy(x => 1);

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

        return sortedTrans;
    }

    public void Dispose()
    {
        TransactionsState.OnTransactionHistoryChanged -= HandleTransactionHistoryChanged;
        LookupListState.PropertyChanged -= HandlePropertyChanged;
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