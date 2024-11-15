using BibleWell.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace BibleWell.App.ViewModels;

public partial class OpenFileViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _fileText;

    [RelayCommand]
    private async Task OpenFileWithMauiFilePicker(CancellationToken ct)
    {
        ErrorMessages?.Clear();
        try
        {
            var filesService = Ioc.Default.GetService<IFilesService>();
            if (filesService is null)
            {
                throw new NullReferenceException(nameof(filesService));
            }

            var file = await filesService.OpenFileAsync();
            if (file is null)
            {
                return;
            }

            await using var readStream = await file.OpenReadAsync();
            using var reader = new StreamReader(readStream);
            FileText = await reader.ReadToEndAsync(ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}