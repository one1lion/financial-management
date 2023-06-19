using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
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

    private ResponseModel<ILookupItemViewModel>? _response = null;

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
            RefreshItems();
            _loadingList = false;
        }
        await base.SetParametersAsync(ParameterView.Empty);
    }

    private void RefreshItems()
    {
        _items = _lookupType switch
        {
            LookupListType.AccountTypes => LookupListState.GetLookupItems<LookupItemViewModel<LuAccountType>>().ToList<ILookupItemViewModel>(),
            LookupListType.Categories => LookupListState.GetLookupItems<LookupItemViewModel<LuCategory>>().ToList<ILookupItemViewModel>(),
            LookupListType.DepositReasons => LookupListState.GetLookupItems<LookupItemViewModel<LuDepositReason>>().ToList<ILookupItemViewModel>(),
            LookupListType.LineItemTypes => LookupListState.GetLookupItems<LookupItemViewModel<LuLineItemType>>().ToList<ILookupItemViewModel>(),
            LookupListType.Payees => LookupListState.GetLookupItems<PayeeLookupViewModel>().ToList<ILookupItemViewModel>(),
            _ => new()
        };

        if (_lookupType == LookupListType.Payees)
        {
            _payeeCategories = LookupListState.LookupItemCache
                .OfType<LookupItemViewModel<LuCategory>>()
                .ToList();
        }
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
        if (e.Key == "Enter")
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

        if ((_items?.Any(x => x.DisplayText == _newItemName) ?? false))
        {
            _addNewError = "Item already exists";
            return;
        }

        _items ??= new();

        // Do the service call to create a new item
        switch (_lookupType)
        {
            case LookupListType.AccountTypes:
                {
                    _response = await LookupListState.CreateLookupItemAsync(new LookupItemViewModel<LuAccountType>(new LuAccountType { Name = _newItemName }));
                }
                break;
            case LookupListType.Categories:
                {
                    _response = await LookupListState.CreateLookupItemAsync(new LookupItemViewModel<LuCategory>(new LuCategory { Name = _newItemName }));
                }
                break;
            case LookupListType.DepositReasons:
                {
                    _response = await LookupListState.CreateLookupItemAsync(new LookupItemViewModel<LuDepositReason>(new LuDepositReason { Name = _newItemName }));
                }
                break;
            case LookupListType.LineItemTypes:
                {
                    _response = await LookupListState.CreateLookupItemAsync(new LookupItemViewModel<LuLineItemType>(new LuLineItemType { Name = _newItemName }));
                }
                break;
            case LookupListType.Payees:
                {
                    _response = await LookupListState.CreateLookupItemAsync(new PayeeLookupViewModel(new Payee { Name = _newItemName, Categories = _selectedCategories.Select(x => x.Item).ToList() }));
                }
                break;
            default:
                _addNewError = $"Error creating new item.  Cannot add an item of Type {_lookupType}.";
                return;
        }

        if (_response is null)
        {
            _addNewError = "An error occurred while receiving the response from the server.  Please refresh the page to determine if the item was successfully added.";
            return;
        }

        if (_response.WasError)
        {
            // The error will be displayed in the UI
            return;
        }

        if(_response.Data is null)
        {
            _addNewError = "An error occurred while receiving the response from the server.  Please refresh the page to determine if the item was successfully added.";
            return;
        }

        // If we are here, the item was successfully created
        _items.Add(_response.Data);

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