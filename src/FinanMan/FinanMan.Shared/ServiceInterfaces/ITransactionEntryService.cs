using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;

namespace FinanMan.Shared.ServiceInterfaces;

public interface ITransactionEntryService { }

public interface ITransactionEntryService<TDataEntryViewModel> : ITransactionEntryService
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    Task<ResponseModel<List<TDataEntryViewModel>>?> GetTransactionsAsync(ushort startRecord = 0, ushort pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default);
    Task<ResponseModel<TDataEntryViewModel>?> GetTransactionAsync(int id, CancellationToken ct = default);
    Task<ResponseModelBase<int>> AddTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> UpdateTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> DeleteTransactionAsync(int id, CancellationToken ct = default);
}
