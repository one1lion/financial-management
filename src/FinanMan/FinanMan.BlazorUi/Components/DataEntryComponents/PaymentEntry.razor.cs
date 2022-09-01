using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class PaymentEntry
{
    private PaymentEntryViewModel _newPayment = new();
    private LineItemViewModel _newLineItem = new();
    private List<Account>? _accounts;
    private List<LuLineItemType>? _lineItemTypes;
    private List<Payee>? _payees;

    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(2000);
        _accounts = new()
        {
            new() {Id = 1, Name = "Checking"},
            new() {Id = 2, Name = "Savings"},
            new() {Id = 3, Name = "Cheese"},
            new() {Id = 4, Name = "Coffee"}
        };
        _payees = new()
        {
            new() {Id = 1, Name = "Vindy's"}, 
            new() {Id = 2, Name = "My Favourite Groucerie Stoure"}, 
            new() {Id = 3, Name = "Tacho Bear"}
        };
        _lineItemTypes = new()
        {
            new() {Id = 1, Name = "Sub Total", SortOrder = 0}, 
            new() {Id = 2, Name = "Tax", SortOrder = 1}, 
            new() {Id = 3, Name = "Tip", SortOrder = 2}
        };
    }

    private void HandleAddDetailClicked()
    {
        _newPayment.LineItems.Add(_newLineItem);
        _newLineItem = new();
    }
}