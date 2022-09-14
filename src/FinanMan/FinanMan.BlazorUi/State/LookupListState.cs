using FinanMan.Database.Models.Tables;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.StateInterfaces;

namespace FinanMan.BlazorUi.State;

public class LookupListState : ILookupListState
{
    public List<ILookupItemViewModel> LookupItemCache { get; } = new();

    private bool _initialized;
    private bool _initializing;

    public bool Initialized { get => _initialized; set => _initialized = value; }
    public bool Initializing { get => _initializing; set => _initializing = value; }

    public Task Initialize()
    {
        if (_initialized || _initializing) { return Task.CompletedTask; }
        _initializing = true;
        List<Account> _accounts = new()
        {
            new() {Id = 1, Name = "Checking"},
            new() {Id = 2, Name = "Savings"},
            new() {Id = 3, Name = "Cheese"},
            new() {Id = 4, Name = "Coffee"}
        };
        List<LuDepositReason> _depositReasons = new()
        {
            new() {Id = 1, Name = "Regular Paycheck"},
            new() {Id = 2, Name = "State Income Tax Return"},
            new() {Id = 3, Name = "Federal Income Tax Return"},
            new() {Id = 4, Name = "Just Because"}
        };
        List<LuLineItemType> _lineItemTypes = new()
        {
            new() {Id = 1, Name = "Sub Total", SortOrder = 0},
            new() {Id = 2, Name = "Tax", SortOrder = 1},
            new() {Id = 3, Name = "Tip", SortOrder = 2}
        };

        List<Payee> _payees = new()
        {
            new() {Id = 1, Name = "Vindy's"},
            new() {Id = 2, Name = "My Favourite Groucerie Stoure"},
            new() {Id = 3, Name = "Tacho Bear"}
        };

        // API request to get list items
        LookupItemCache.AddRange(_accounts.Select(x => new LookupItemViewModel<AccountViewModel>()
        {
            Id = x.Id,
            DisplayText = x.Name,
            ValueText = x.Id.ToString(),
            LastUpdated = DateTime.UtcNow,
            Item = x.ToViewModel()
        }));

        LookupItemCache.AddRange(_depositReasons.Select(x => new LookupItemViewModel<LuDepositReason>()
        {
            Id = x.Id,
            DisplayText = x.Name,
            ValueText = x.Id.ToString(),
            LastUpdated = DateTime.UtcNow,
            Item = x
        }));

        LookupItemCache.AddRange(_lineItemTypes.Select(x => new LookupItemViewModel<LuLineItemType>()
        {
            Id = x.Id,
            DisplayText = x.Name,
            ValueText = x.Id.ToString(),
            LastUpdated = DateTime.UtcNow,
            Item = x
        }));

        LookupItemCache.AddRange(_payees.Select(x => new LookupItemViewModel<PayeeViewModel>()
        {
            Id = x.Id,
            DisplayText = x.Name,
            ValueText = x.Id.ToString(),
            LastUpdated = DateTime.UtcNow,
            Item = x.ToViewModel()
        }));

        _initialized = true;
        _initializing = false;

        return Task.CompletedTask;
    }

    public IEnumerable<ILookupItemViewModel<TKey, TLookupItem>> GetLookupItems<TKey, TLookupItem>()
    {
        return LookupItemCache.OfType<LookupItemViewModel<TKey, TLookupItem>>();
    }
}
