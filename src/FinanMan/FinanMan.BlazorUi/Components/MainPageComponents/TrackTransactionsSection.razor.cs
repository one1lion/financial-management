namespace FinanMan.BlazorUi.Components.MainPageComponents;
public partial class TrackTransactionsSection
{
    [Parameter] public EventCallback<MouseEventArgs> OnGettingStartedClicked { get; set; }

    private Task HandleGetStartedClicked(MouseEventArgs e)
    {
        Console.WriteLine("TrackTransactionSection: Getting Started was clicked");

        return OnGettingStartedClicked.HasDelegate
                ? OnGettingStartedClicked.InvokeAsync(e)
                : Task.CompletedTask;
    }
}
