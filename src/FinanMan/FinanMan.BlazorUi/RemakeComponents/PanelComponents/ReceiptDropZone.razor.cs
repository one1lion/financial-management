namespace FinanMan.BlazorUi.RemakeComponents.PanelComponents;

public partial class ReceiptDropZone
{
    [Inject, AllowNull] private IJSRuntime JSRuntime { get; set; } = null!;
    [Parameter] public EventCallback<IBrowserFile> OnFileUploaded { get; set; }

    private bool IsDragging { get; set; }
    private IBrowserFile? UploadedFile { get; set; }
    private string? UploadedFileUrl { get; set; }

    private void HandleDragEnter()
    {
        IsDragging = true;
    }

    private void HandleDragLeave()
    {
        IsDragging = false;
    }

    private Task HandleDrop(DragEventArgs e)
    {
        IsDragging = false;
        // In a real implementation, this would handle the dropped files
        // For now, this is just a placeholder
        return Task.CompletedTask;
    }

    private async Task BrowseFiles()
    {
        await JSRuntime.InvokeVoidAsync("document.getElementById('fileInput').click");
    }

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            UploadedFile = file;

            // In a real implementation, we would process the file
            // For now, just mock the URL for images
            if (IsImage(file.ContentType))
            {
                // Simulate a file URL for preview
                UploadedFileUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==";
            }

            await OnFileUploaded.InvokeAsync(file);
        }
    }

    private void RemoveFile()
    {
        UploadedFile = null;
        UploadedFileUrl = null;
    }

    private bool IsImage(string contentType)
    {
        return contentType.StartsWith("image/");
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}
