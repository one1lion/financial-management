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

    private ElementReference? _newItemInputRef;
    private string? _newItemName;
    private List<LookupItemViewModel<LuCategory>> _payeeCategories = new();
    private List<LookupItemViewModel<LuCategory>> _selectedCategories = new();
    private string? _addNewError;

    protected override void OnInitialized()
    {
        LookupListState.PropertyChanged += HandleLookupListTypeChanged;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (parameters.TryGetValue<LookupListType>(nameof(LookupType), out var lt) && lt != _lookupType)
        {
            _lookupType = lt;
            _loadingList = true;

            await LookupListState.InitializeAsync();
            _items = lt switch
            {
                LookupListType.AccountTypes => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuAccountType>>().ToList<ILookupItemViewModel>(),
                LookupListType.Categories => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuCategory>>().ToList<ILookupItemViewModel>(),
                LookupListType.DepositReasons => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuDepositReason>>().ToList<ILookupItemViewModel>(),
                LookupListType.LineItemTypes => LookupListState.LookupItemCache.OfType<LookupItemViewModel<LuLineItemType>>().ToList<ILookupItemViewModel>(),
                LookupListType.Payees => LookupListState.LookupItemCache.OfType<PayeeLookupViewModel>().ToList<ILookupItemViewModel>(),
                _ => new()
            };

            if(lt == LookupListType.Payees)
            {
                _payeeCategories = LookupListState.LookupItemCache
                    .OfType<LookupItemViewModel<LuCategory>>()
                    .ToList();
            }
            _loadingList = false;
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

    private Task HandleKeyDown(KeyboardEventArgs e)
    {
        if(e.Key == "Enter")
        {
            return HandleAddNewItemClicked();
        }

        return Task.CompletedTask;
    }

    private async Task HandleAddNewItemClicked()
    {
        _addNewError = string.Empty;
        if (string.IsNullOrWhiteSpace(_newItemName))
        {
            return;
        }

        if((_items?.Any(x => x.DisplayText == _newItemName) ?? false))
        {
            _addNewError = "Item already exists";
            return;
        }

        _items ??= new();

        switch (_lookupType)
        {
            case LookupListType.AccountTypes:
                _items.Add(new LookupItemViewModel<LuAccountType>(new LuAccountType { Name = _newItemName }));
                break;
            case LookupListType.Categories:
                _items.Add(new LookupItemViewModel<LuCategory>(new LuCategory { Name = _newItemName }));
                break;
            case LookupListType.DepositReasons:
                _items.Add(new LookupItemViewModel<LuDepositReason>(new LuDepositReason { Name = _newItemName }));
                break;
            case LookupListType.LineItemTypes:
                _items.Add(new LookupItemViewModel<LuLineItemType>(new LuLineItemType { Name = _newItemName }));
                break;
            case LookupListType.Payees:
                _items.Add(new PayeeLookupViewModel(new Payee { Name = _newItemName, Categories = _selectedCategories.Select(x => x.Item).ToList() }));
                break;
            default:
                break;
        }

        // TODO: If the process was successful, clear the new item name and selected categories
        _newItemName = string.Empty;
        _selectedCategories.Clear();
        if (_newItemInputRef.HasValue)
        {
            await _newItemInputRef.Value.FocusAsync();
        }
        return;
    }
}