using FinanMan.Database.Models.Tables;
using FinanMan.Shared.LookupModels;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.ListManagementComponents;

public partial class LookupListEdit
{
    [Inject, AllowNull] private ILookupListState LookupListState { get; set; }

    [Parameter] public LookupListType LookupType { get; set; }
    private LookupListType? _lookupType;
    private List<ILookupItemViewModel>? _items;
    private bool _loadingList;

    protected override void OnInitialized()
    {
        LookupListState.PropertyChanged += HandleLookupListTypeChanged;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        Console.WriteLine($"Set Parameters for LookupListEdit");
        await base.SetParametersAsync(parameters);
        if (parameters.TryGetValue<LookupListType>(nameof(LookupType), out var lt) && lt != _lookupType)
        {
            _lookupType = lt;
            Console.WriteLine($"Loading edit list for {lt.ToString()}");
            _loadingList = true;
            _items = lt switch
            {
                LookupListType.AccountTypes => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuAccountType>>().ToList<ILookupItemViewModel>(),
                LookupListType.Categories => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuCategory>>().ToList<ILookupItemViewModel>(),
                LookupListType.DepositReasons => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuDepositReason>>().ToList<ILookupItemViewModel>(),
                LookupListType.LineItemTypes => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuLineItemType>>().ToList<ILookupItemViewModel>(),
                LookupListType.Payees => LookupListState.LookupItemCache.OfType<PayeeLookupViewModel>().ToList<ILookupItemViewModel>(),
                _ => new()
            };
            Console.WriteLine($"Count of Items found: {_items?.Count.ToString() ?? "NaN"}");
            _loadingList = false;
        }
        else
        {
            Console.WriteLine("I didn't find a lookup list type param value.");
        }
        await base.SetParametersAsync(ParameterView.Empty);
    }

    private async void HandleLookupListTypeChanged(object? sender, PropertyChangedEventArgs e)
    {
        await InvokeAsync(StateHasChanged);
    }

    private Task HandleEditItemClicked(ILookupItemViewModel item)
    {
        return Task.CompletedTask;
    }

    private Task HandleDeleteItemClicked(ILookupItemViewModel item)
    {
        return Task.CompletedTask;
    }
}