using Microsoft.Maui.Storage;

namespace BibleWell.App.Services;

public interface IFilesService
{
    public Task<FileResult?> OpenFileAsync();
}