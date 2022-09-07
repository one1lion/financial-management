using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class DepositEntry
{
    private DepositViewModel _newDeposit = new();
    private List<Account>? _accounts;
    private List<LuDepositReason>? _depositReasons;
    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(2000);
        _accounts = new()
        {new()
        {Id = 1, Name = "Checking"}, new()
        {Id = 2, Name = "Savings"}, new()
        {Id = 3, Name = "Cheese"}, new()
        {Id = 4, Name = "Coffee"}};
        _depositReasons = new()
        {new()
        {Id = 1, Name = "Regular Paycheck"}, new()
        {Id = 2, Name = "State Income Tax Return"}, new()
        {Id = 3, Name = "Federal Income Tax Return"}, new()
        {Id = 4, Name = "Just Because"}};
    }
}
