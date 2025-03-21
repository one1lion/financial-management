# FinanMan State Management Guide

## Overview

FinanMan uses a state management pattern to maintain application state across components. This approach centralizes data access and manages the UI state throughout the application.

## State Service Architecture

The state management system is based on three primary services:

1. **ILookupListState** - Manages lookup list data (categories, account types, etc.)
2. **ITransactionsState** - Manages transaction data and operations
3. **IUiState** - Manages UI-specific state (dialogs, flyouts, language)

All state services implement `INotifyPropertyChanged` through a base class called `BaseNotifyPropertyChanges` which provides:

- Property change notifications
- Thread-safe property updates
- Consistent state update patterns

## LookupListState

### Purpose

Manages a cache of lookup items used throughout the application:

- Account types
- Categories
- Deposit reasons
- Line item types
- Payees

### Key Methods

```csharp
// Initialize the lookup list cache
Task InitializeAsync();

// Get lookup items of a specific type
IEnumerable<T> GetLookupItems<T>() where T : class, ILookupItemViewModel, IHasLookupListType, new();

// Create a new lookup item
Task<ResponseModel<ILookupItemViewModel>> CreateLookupItemAsync<T>(T lookupItem) 
    where T : class, ILookupItemViewModel, IHasLookupListType, new();

// Refresh a specific lookup list
Task RefreshListAsync<T>() where T : class, ILookupItemViewModel, IHasLookupListType, new();

// Update a lookup item
Task<ResponseModel<ILookupItemViewModel>> UpdateLookupItemAsync<T>(T lookupItem, CancellationToken ct = default)
    where T : class, ILookupItemViewModel, IHasLookupListType, new();

// Delete a lookup item
Task<ResponseModelBase<int>> DeleteLookupItemAsync<T>(T lookupItem, CancellationToken ct)
    where T : class, ILookupItemViewModel, IHasLookupListType, new();
```

### Usage Example

```csharp
@inject ILookupListState LookupListState

@code {
    private List<AccountLookupViewModel>? _accounts;
    private List<LookupItemViewModel<LuDepositReason>>? _depositReasons;

    protected override async Task OnInitializedAsync()
    {
        await LookupListState.InitializeAsync();
        _accounts = LookupListState.GetLookupItems<AccountLookupViewModel>().ToList();
        _depositReasons = LookupListState.GetLookupItems<LookupItemViewModel<LuDepositReason>>().ToList();
    }
}
```

## TransactionsState

### Purpose

Manages transaction data and operations:

- Fetching transactions
- Tracking transaction changes
- Notifying components of updates

### Key Methods

```csharp
// Refresh transaction history
Task RefreshTransactionHistoryAsync();

// Remove a transaction from the state
bool RemoveTransaction(int transactionId);

// Notify that transactions have changed
void NotifyTransactionsChanged();
```

### Events

```csharp
// Fired when transaction history changes
event Func<Task>? OnTransactionHistoryChanged;

// Fired when transaction refresh encounters errors
event Func<List<ResponseModel<List<ITransactionDataEntryViewModel>>>, Task>? OnTransactionRefreshError;
```

### Usage Example

```csharp
@inject ITransactionsState TransactionsState

@code {
    private IEnumerable<ITransactionDataEntryViewModel>? _transactions;

    protected override void OnInitialized()
    {
        TransactionsState.OnTransactionHistoryChanged += HandleTransactionHistoryChanged;
    }

    private async Task HandleTransactionHistoryChanged()
    {
        _transactions = TransactionsState.Transactions;
        await InvokeAsync(StateHasChanged);
    }

    private async Task RefreshTransactions()
    {
        await TransactionsState.RefreshTransactionHistoryAsync();
    }

    public void Dispose()
    {
        TransactionsState.OnTransactionHistoryChanged -= HandleTransactionHistoryChanged;
    }
}
```

## UiState

### Purpose

Manages UI-specific state:

- Language and localization
- Dialog visibility
- Flyout content and visibility
- Global UI events

### Key Properties

```csharp
// Flyout content and visibility
RenderFragment? FlyoutContent { get; }
bool FlyoutVisible { get; }

// Active language
string ActiveLanguage { get; }

// Message dialog
bool MessageDialogVisible { get; set; }
MessageDialogParameters MessageDialogParameters { get; set; }
```

### Key Methods

```csharp
// Set active language
void SetLanguage(string language);

// Display/hide flyouts
void DisplayFlyout(RenderFragment? content);
void CollapseFlyout();

// Show message dialog
void ShowMessageDialog(RenderFragment message, RenderFragment? title = null, RenderFragment? okButtonCaption = null);

// Handle UI events
void CollapseAllSelectLists();
void RaiseInitialUiLoadComplete();
```

### Events

```csharp
// Language changed event
event Func<Task>? ActiveLanguageChanged;

// UI events
event Func<Task>? CollapseSelectLists;
event Func<Task>? InitialUiLoadComplete;
```

### Usage Example

```csharp
@inject IUiState UiState

@code {
    private void HandleShowFlyout()
    {
        UiState.DisplayFlyout(@<MyFlyoutContent />);
    }

    private void HandleShowDialog()
    {
        UiState.ShowMessageDialog(
            message: @<p>This is a message</p>,
            title: @<h3>Dialog Title</h3>
        );
    }
}
```

## State Update Patterns

### Setting Fields with Notification

```csharp
private bool _flyoutVisible;
public bool FlyoutVisible { 
    get => _flyoutVisible; 
    private set => SetField(ref _flyoutVisible, value); 
}
```

### Manual Property Changed Notification

```csharp
public List<ILookupItemViewModel> LookupItemCache { get; } = new();

public void AddLookupItem(ILookupItemViewModel item)
{
    LookupItemCache.Add(item);
    RaisePropertyChanged(nameof(LookupItemCache));
}
```

### Event-Based Updates

```csharp
public event Func<Task>? OnTransactionHistoryChanged;

public void NotifyTransactionsChanged()
{
    OnTransactionHistoryChanged?.Invoke();
}
```

## Best Practices

1. **Subscribe in OnInitialized** - Subscribe to state events in OnInitialized
2. **Unsubscribe in Dispose** - Always unsubscribe in the Dispose method
3. **Use InvokeAsync** - Always use InvokeAsync when updating component state from events
4. **Keep state immutable** - Don't modify state objects directly
5. **Use cancellation tokens** - For operations that may be interrupted
6. **Handle errors** - Always handle errors in state operations
7. **Limit state dependencies** - Components should only depend on the state they need

## Common Pitfalls

1. **Memory Leaks** - Forgetting to unsubscribe from events
2. **Threading Issues** - Not using InvokeAsync when updating state from events
3. **Cascading Updates** - Too many components subscribing to the same state
4. **Stale State** - Not refreshing state when needed
5. **Direct Modification** - Modifying state objects directly instead of through state services