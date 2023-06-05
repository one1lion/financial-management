using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.ListManagementComponents;
public partial class ListManagementPage
{
    [Inject, AllowNull] private NavigationManager NavigationManager { get; set; }
    [Parameter] public LookupListType ListType { get; set; } = LookupListType.AccountTypes;

    private void HandleListTypeSelected(LookupListType listType)
    {
        NavigationManager.NavigateTo($"data-entry/{EntryType.ListManagement}/{listType}");
        ListType = listType;
        StateHasChanged();
    }
}
