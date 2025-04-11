using BibleWell.Aquifer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;
#if ANDROID || IOS
using Microsoft.Maui.Storage;
#endif

namespace BibleWell.App.ViewModels.Pages;

public sealed partial class ResourcesPageViewModel(ICachingAquiferService _cachingAquiferService)
    : PageViewModelBase
{
    [ObservableProperty]
    private string _resourceContent = "Click the button to view resource text...";

    [RelayCommand]
    public async Task PopulateResourceContentAsync()
    {
        try
        {
            string? fileName = null;
#if ANDROID || IOS
            var file = await FilePicker.PickAsync();
            fileName = file?.FileName;
#endif
            ResourceContent = fileName is null
                ? (await _cachingAquiferService.GetResourceAsync(42))
                    ?.Content
                    ?? "Resource not found."
                : $"Found file: \"{fileName}\"";

            await TextToSpeech.Default.SpeakAsync(ResourceContent);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}