using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using System.ComponentModel;

namespace FinanMan.Shared.StateInterfaces;
public interface ITransactionsState : INotifyPropertyChanged, INotifyPropertyChanging
{
    event Func<Task>? OnTransactionHistoryChanged;
    event Func<List<ResponseModel<List<ITransactionDataEntryViewModel>>>, Task>? OnTransactionRefreshError;

    List<ITransactionDataEntryViewModel>? Transactions { get; set; }
    
    Task RefreshTransactionHistoryAsync();
    void NotifyTransactionsChanged();
}
