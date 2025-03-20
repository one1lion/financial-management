# FinanMan API Usage Guide

This guide demonstrates how to use the core APIs in the FinanMan library for common operations.

## Setting Up Dependencies

Both client and server applications need to register the appropriate services:

### Server-side Registration

```csharp
// Program.cs
builder.Services
    .SetupDbContext(config)                  // Registers DbContext
    .AddFinanManLocalization()               // Adds localization support
    .AddServerServices()                     // Registers server-side service implementations
    .AddFluentValidation();                  // Registers validators
```

### Client-side Registration

```csharp
// Program.cs for WASM or similar
builder.Services
    .AddClientServices()                     // Registers client-side service implementations
    .AddFinanManLocalization()               // Adds localization support
    .AddFluentValidation();                  // Registers validators
```

## Working with Transactions

### Fetching Transactions

```csharp
// Inject the appropriate service
@inject ITransactionEntryService<DepositEntryViewModel> DepositService

// In your code
var response = await DepositService.GetTransactionsAsync(
    startRecord: 0,      // Pagination start
    pageSize: 100,       // Pagination size
    asOfDate: null,      // Optional filter by date
    includeMarkedAsPurge: false  // Whether to include soft-deleted records
);

if (!response.WasError) 
{
    var transactions = response.Data;
    // Process transactions...
}
else
{
    // Handle errors
    foreach (var error in response.ErrorMessages)
    {
        Console.WriteLine(error);
    }
}
```

### Adding a Transaction

```csharp
// Create a new deposit transaction
var deposit = new DepositEntryViewModel
{
    TransactionDate = DateTime.Today,
    AccountId = 1,
    DepositReasonValueText = "2",  // ID of the deposit reason
    Amount = 100.00m,
    Memo = "Paycheck"
};

// Save the transaction
var result = await DepositService.AddTransactionAsync(deposit);

if (!result.WasError)
{
    int newId = result.RecordId;
    // Transaction saved successfully
}
```

### Updating a Transaction

```csharp
// First fetch the transaction
var getResponse = await PaymentService.GetTransactionAsync(id);
if (getResponse.WasError || getResponse.Data == null) return;

var payment = getResponse.Data;

// Modify the transaction
payment.Amount = 150.00m;
payment.Memo = "Updated description";

// Save the changes
var updateResult = await PaymentService.UpdateTransactionAsync(payment);

if (!updateResult.WasError)
{
    // Transaction updated successfully
}
```

### Deleting a Transaction

```csharp
var deleteResult = await TransferService.DeleteTransactionAsync(id);

if (!deleteResult.WasError)
{
    // Transaction marked for deletion successfully
}
```

## Working with Accounts

### Fetching Accounts

```csharp
@inject IAccountService AccountService

// Get all accounts
var accountsResponse = await AccountService.GetAccountsAsync();

if (!accountsResponse.WasError && accountsResponse.Data != null)
{
    var accounts = accountsResponse.Data;
    // Process accounts...
}

// Get account summaries (with balance information)
var summariesResponse = await AccountService.GetAccountSummariesAsync();

if (!summariesResponse.WasError && summariesResponse.Data != null)
{
    var summaries = summariesResponse.Data;
    // Process account summaries...
}
```

### Creating an Account

```csharp
var newAccount = new AccountLookupViewModel
{
    DisplayText = "Checking Account",
    AccountTypeId = 1  // ID for checking account type
};

var result = await AccountService.CreateAccountAsync(newAccount);

if (!result.WasError)
{
    int newAccountId = result.RecordId;
    // Account created successfully
}
```

## Working with Lookup Lists

### Fetching Lookup Items

```csharp
@inject ILookupListService LookupService
@inject ILookupListState LookupState  // For using cached lookups

// Option 1: Direct service call
var response = await LookupService.GetLookupItemsAsync<LookupItemViewModel<LuCategory>>();

if (!response.WasError && response.Data != null)
{
    var categories = response.Data;
    // Process categories...
}

// Option 2: Using the state service (preferred, uses cache)
var categories = LookupState.GetLookupItems<LookupItemViewModel<LuCategory>>();
```

### Adding a Lookup Item

```csharp
var newCategory = new LookupItemViewModel<LuCategory>
{
    DisplayText = "Groceries",
    SortOrder = 5
};

var result = await LookupService.CreateLookupItemAsync(newCategory);

if (!result.WasError)
{
    int newCategoryId = result.RecordId;
    // Category created successfully
    
    // Refresh the cached lookup items
    await LookupState.RefreshListAsync<LookupItemViewModel<LuCategory>>();
}
```

## Handling the Response Model

All service methods return a `ResponseModel` that should be checked for errors:

```csharp
var response = await service.SomeOperationAsync();

if (response.WasError)
{
    // Option 1: Log or display error messages
    foreach (var error in response.ErrorMessages ?? Enumerable.Empty<string>())
    {
        Console.WriteLine(error);
    }
    
    // Option 2: Use the RenderError fragment to show errors in UI
    <div class="errors">
        @response.RenderError
    </div>
    
    // Option 3: Convert errors to HTML
    string errorHtml = response.AsHtml();
    
    return; // Handle the error case
}

// Process successful response
var data = response.Data;
```

## Working with Validation

The application uses FluentValidation, which is automatically called by the service layer:

```csharp
// Validators are registered in the DI container
services.AddSingleton<TransactionViewModelValidator<DepositEntryViewModel>, DepositEntryViewModelValidator>();

// When you call a service method, validation happens automatically
var result = await DepositService.AddTransactionAsync(deposit);

if (result.WasError)
{
    // Check for validation failures
    if (result.ValidationFailures?.Any() == true)
    {
        foreach (var failure in result.ValidationFailures)
        {
            Console.WriteLine($"{failure.PropertyName}: {failure.ErrorMessage}");
        }
    }
}
```

## Localization

To use localized strings:

```csharp
@inject IStringLocalizer<SharedLocalizerService> Localizer

<h1>@Localizer["AccountSummary"]</h1>
<button>@Localizer["Save"]</button>

@code {
    public string GetLocalizedTitle()
    {
        return Localizer["TransactionHistory"];
    }
}
```

For enums with display attributes:

```csharp
// Get display text for an enum value
var displayText = myEnum.GetDisplayText();

// Get short name version
var shortName = myEnum.GetDisplayText(shortVersion: true);
```