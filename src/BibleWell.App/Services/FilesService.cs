using Microsoft.Maui.Storage;

namespace BibleWell.App.Services;

public class FilesService : IFilesService
{
    public async Task<FileResult?> OpenFileAsync()
    {
        return await FilePicker.PickAsync();
    }
}