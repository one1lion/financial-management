using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.Shared.DataEntryModels;

public class AccountEntryViewModel
{
    public int Id { get; set; }
    [Required]
    public string? AccountName { get; set; }
    [Required]
    public string? AccountTypeValueText { get; set; }
    [Required]
    public DateTime? DateOpened { get; set; }
    [Required]
    public decimal? StartingBalance { get; set; }
}

public static class AccountEntryViewModelExtensions
{
    public static AccountLookupViewModel ToLookupViewModel(this AccountEntryViewModel model)
    {
        var accountTypeId = int.TryParse(model.AccountTypeValueText ?? string.Empty, out var atid) ? atid : 0;
        return new Account()
        {
            Name = model.AccountName ?? string.Empty,
            AccountTypeId = accountTypeId,
            AccountType = new()
            {
                Id = accountTypeId,
                Name = string.Empty
            },
        }.ToViewModel();
    }
}
