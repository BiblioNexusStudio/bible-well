using System.Collections.ObjectModel;
using BibleWell.App.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BibleWell.App.ViewModels;

public sealed partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty]
    private bool _isMenuPaneOpen = true;

    [ObservableProperty]
    private MenuItemTemplate? _selectedMenuItem;

    public ObservableCollection<MenuItemTemplate> MenuItems { get; } =
    [
        new(typeof(HomePageViewModel)),
        new(typeof(BiblePageViewModel)),
        new(typeof(GuidePageViewModel)),
        new(typeof(ResourcesPageViewModel)),
        new(typeof(LibraryPageViewModel)),
    ];

    partial void OnSelectedMenuItemChanged(MenuItemTemplate? value)
    {
        if (value == null)
        {
            return;
        }

        CurrentPage = Activator.CreateInstance(value.ViewModelType) as PageViewModelBase
            ?? throw new InvalidOperationException(
                $"{nameof(MenuItemTemplate)}.{nameof(MenuItemTemplate.ViewModelType)} must derive from {nameof(PageViewModelBase)}.");
    }

    [RelayCommand]
    private void TriggerMenuPane()
    {
        IsMenuPaneOpen = !IsMenuPaneOpen;
    }
}