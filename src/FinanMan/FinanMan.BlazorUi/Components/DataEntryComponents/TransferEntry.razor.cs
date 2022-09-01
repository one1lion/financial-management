using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class TransferEntry
{
    private TransferViewModel _newTransfer = new();
    private List<Account>? _accounts;
    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(2000);
        _accounts = new()
        {new()
        {Id = 1, Name = "Checking"}, new()
        {Id = 2, Name = "Savings"}, new()
        {Id = 3, Name = "Cheese"}, new()
        {Id = 4, Name = "Coffee"}};
    }
}