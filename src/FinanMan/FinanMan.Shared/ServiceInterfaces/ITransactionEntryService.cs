using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;

namespace FinanMan.Shared.ServiceInterfaces;

public interface ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionData(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default);
    Task<ResponseModel<TDataEntryViewModel>> GetTransactionData(int id, CancellationToken ct = default);
    Task<ResponseModelBase<int>> AddTransactionData(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> UpdateTransactionData(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> DeleteTransactionData(int id, CancellationToken ct = default);
}
