using BibleWell.Aquifer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BibleWell.App.ViewModels.Pages;

/// <summary>
/// Design-time ViewModel for use with the <see cref="Views.Pages.ResourcesPageView"/>.
/// </summary>
public sealed class DesignResourcesPageViewModel() : ResourcesPageViewModel(new FakeCachingAquiferService());

/// <summary>
/// ViewModel for use with the <see cref="Views.Pages.ResourcesPageView"/>.
/// </summary>
public partial class ResourcesPageViewModel(ICachingAquiferService _cachingAquiferService)
    : PageViewModelBase
{
    [ObservableProperty]
    private string _resourceContentHtml = "<p>Click the button to view resource text...</p>";

    [RelayCommand]
    public async Task PopulateResourceContentAsync()
    {
        try
        {
            ResourceContentHtml = (await _cachingAquiferService.GetResourceContentAsync(366960))
                ?.Content
                ?? "Resource not found.";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}