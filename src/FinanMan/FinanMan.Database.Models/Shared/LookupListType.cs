using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FinanMan.Database.Models.Shared;

public enum LookupListType
{
    [Display(Name = "AccountsTitleDisplay", ShortName = "Accounts", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    Accounts,
    [Display(Name = "AccountTypesTitleDisplay", ShortName = "AccountTypes", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    AccountTypes,
    [Display(Name = "CategoriesTitleDisplay", ShortName = "Categories", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    Categories,
    [Display(Name = "DepositReasonsTitleDisplay", ShortName = "DepositReasons", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    DepositReasons,
    [Display(Name = "LineItemTypesTitleDisplay", ShortName = "LineItemTypes", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    LineItemTypes,
    [Display(Name = "PayeesTitleDisplay", ShortName = "Payees", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    Payees,
    [Display(Name = "RecurrenceTypesTitleDisplay", ShortName = "RecurrenceTypes", ResourceType = typeof(Resources.Localization.Enums.LookupListType))]
    RecurrenceTypes
}

public static class LookupListTypeExtensions
{
    private readonly static LookupListType[] _excludedFromManagement = new[] {
        LookupListType.Accounts,
        LookupListType.RecurrenceTypes
    };

    public static bool IsExcluded(this LookupListType listType)
        => _excludedFromManagement.Contains(listType);
}
