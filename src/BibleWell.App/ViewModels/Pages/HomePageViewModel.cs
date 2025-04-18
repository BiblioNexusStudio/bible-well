using Avalonia;
using Avalonia.Styling;
using BibleWell.Preferences;
using CommunityToolkit.Mvvm.Input;

namespace BibleWell.App.ViewModels.Pages;

/// <summary>
/// Design-time ViewModel for use with the <see cref="Views.Pages.HomePageView"/>.
/// </summary>
public sealed class DesignHomePageViewModel() : HomePageViewModel(new FakeUserPreferencesService());

/// <summary>
/// ViewModel for use with the <see cref="Views.Pages.HomePageView"/>.
/// </summary>
public partial class HomePageViewModel(IUserPreferencesService _userPreferencesService) : PageViewModelBase
{
    [RelayCommand]
    public void ChangeTheme()
    {
        var newThemeVariant = Application.Current!.ActualThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;

        Application.Current!.RequestedThemeVariant = newThemeVariant;

        _userPreferencesService.Set(PreferenceKeys.ThemeVariant, newThemeVariant.ToString());
    }
}