using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;

namespace FinanMan.Shared.ServiceInterfaces;

public interface ITransactionEntryService { }

public interface ITransactionEntryService<TDataEntryViewModel> : ITransactionEntryService
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionDataAsync(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default);
    Task<ResponseModel<TDataEntryViewModel>> GetTransactionDataAsync(int id, CancellationToken ct = default);
    Task<ResponseModelBase<int>> AddTransactionDataAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> UpdateTransactionDataAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> DeleteTransactionDataAsync(int id, CancellationToken ct = default);
}
