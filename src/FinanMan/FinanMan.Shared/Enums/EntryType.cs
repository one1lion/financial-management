using System.ComponentModel.DataAnnotations;

namespace FinanMan.Shared.Enums;

public enum EntryType
{
    [Display(Name = "PaymentsTitleDisplay", ShortName = "Payments", ResourceType = typeof(Resources.Localization.Enums.EntryType))]
    Payments,
    [Display(Name = "DepositsTitleDisplay", ShortName = "Deposits", ResourceType = typeof(Resources.Localization.Enums.EntryType))]
    Deposits,
    [Display(Name = "TransfersTitleDisplay", ShortName = "Transfers", ResourceType = typeof(Resources.Localization.Enums.EntryType))]
    Transfers
}
