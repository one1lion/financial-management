using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.SharedServer.Services
{
    public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
        where TDataEntryViewModel : class, ITransactionDataEntryViewModel
    {
        public Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionData(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<TDataEntryViewModel>> GetTransactionData(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModelBase<int>> AddTransactionData(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModelBase<int>> UpdateTransactionData(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModelBase<int>> DeleteTransactionData(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
