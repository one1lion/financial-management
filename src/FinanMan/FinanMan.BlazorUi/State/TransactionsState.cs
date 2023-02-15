using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.Shared.StateInterfaces;

namespace FinanMan.BlazorUi.State;
public class TransactionsState : BaseNotifyPropertyChanges, ITransactionsState
{
    public TransactionsState(
        ITransactionEntryService<DepositEntryViewModel> depositTransactionService,
        ITransactionEntryService<PaymentEntryViewModel> paymentTransactionService,
        ITransactionEntryService<TransferEntryViewModel> transferTransactionService)
    {
        _depositTransactionService = depositTransactionService;
        _paymentTransactionService = paymentTransactionService;
        _transferTransactionService = transferTransactionService;
    }

    #region Backing fields
    private List<ITransactionDataEntryViewModel>? _transactions;
    private readonly ITransactionEntryService<DepositEntryViewModel> _depositTransactionService;
    private readonly ITransactionEntryService<PaymentEntryViewModel> _paymentTransactionService;
    private readonly ITransactionEntryService<TransferEntryViewModel> _transferTransactionService;
    #endregion Backing Fields

    #region Public Properties
    public List<ITransactionDataEntryViewModel>? Transactions { get => _transactions; set => SetField(ref _transactions, value); }
    #endregion Public Properties

    #region Events
    public event Func<Task>? OnTransactionHistoryChanged;
    public event Func<List<ResponseModel<List<ITransactionDataEntryViewModel>>>, Task>? OnTransactionRefreshError;
    #endregion Events

    #region Public Methods
    public async Task RefreshTransactionHistoryAsync()
    {
        var asOfDate = (_transactions?.Any() ?? false)
            ? _transactions.Max(t => t.DateEntered)
            : default(DateTime?);

        var depTransTask = _depositTransactionService.GetTransactionsAsync(asOfDate: asOfDate);
        var paymentTransTask = _paymentTransactionService.GetTransactionsAsync(asOfDate: asOfDate);
        var transferTransTask = _transferTransactionService.GetTransactionsAsync(asOfDate: asOfDate);

        await Task.WhenAll(depTransTask, paymentTransTask, transferTransTask);

        var depResp = depTransTask.Result;
        var payResp = paymentTransTask.Result;
        var traResp = transferTransTask.Result;

        var transactionErroredResponses = new List<ResponseModel<List<ITransactionDataEntryViewModel>>>();

        if (depResp?.WasError ?? true)
        {
            // TODO: Do something special with the error
            if (depResp is null)
            {
                transactionErroredResponses.Add(new()
                {
                    ErrorMessages = new() { "Error retrieving deposit transactions" }
                });
            }
            else
            {
                transactionErroredResponses.Add(new()
                {
                    Data = depResp.Data?.OfType<ITransactionDataEntryViewModel>().ToList(),
                    ErrorMessages = depResp.ErrorMessages,
                    Exceptions = depResp.Exceptions,
                    ValidationFailures = depResp.ValidationFailures,
                    RecordCount = depResp.RecordCount
                });
            }
        }
        else if (depResp.Data is not null)
        {
            AddTransactionsToList(depResp.Data);
        }

        if (payResp?.WasError ?? true)
        {
            // TODO: Do something special with the error
            if (payResp is null)
            {
                transactionErroredResponses.Add(new()
                {
                    ErrorMessages = new() { "Error retrieving payment transactions" }
                });
            }
            else
            {
                transactionErroredResponses.Add(new()
                {
                    Data = payResp.Data?.OfType<ITransactionDataEntryViewModel>().ToList(),
                    ErrorMessages = payResp.ErrorMessages,
                    Exceptions = payResp.Exceptions,
                    ValidationFailures = payResp.ValidationFailures,
                    RecordCount = payResp.RecordCount
                });
            }
        }
        else if (payResp.Data is not null)
        {
            AddTransactionsToList(payResp.Data);
        }

        if (traResp?.WasError ?? true)
        {
            // TODO: Do something special with the error
            if (traResp is null)
            {
                transactionErroredResponses.Add(new()
                {
                    ErrorMessages = new() { "Error retrieving transfer transactions" }
                });
            }
            else
            {
                transactionErroredResponses.Add(new()
                {
                    Data = traResp.Data?.OfType<ITransactionDataEntryViewModel>().ToList(),
                    ErrorMessages = traResp.ErrorMessages,
                    Exceptions = traResp.Exceptions,
                    ValidationFailures = traResp.ValidationFailures,
                    RecordCount = traResp.RecordCount
                });
            }
        }
        else if (traResp.Data is not null)
        {
            AddTransactionsToList(traResp.Data);
        }
        NotifyTransactionsChanged();
        if (transactionErroredResponses.Any()) { }
        {
            OnTransactionRefreshError?.Invoke(transactionErroredResponses);
        }
    }

    public void NotifyTransactionsChanged()
    {
        OnTransactionHistoryChanged?.Invoke();
    }
    #endregion Public Methods

    #region Private Methods
    private void AddTransactionsToList(IEnumerable<ITransactionDataEntryViewModel> transactions)
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
    #endregion Private Methods

}
