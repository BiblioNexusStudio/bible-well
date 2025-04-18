using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Controls;
using BibleWell.App.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace BibleWell.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        // TODO figure out how to load this in the constructor
        //CurrentPage = GetPageViewModelFromMenuItemTemplate(MenuItems[0]);
    }

    // TODO figure out how to load this in the constructor
    [ObservableProperty]
    private ViewModelBase _currentPage = GetPageViewModelFromMenuItemTemplate(MenuItems[0]);

    [ObservableProperty]
    private bool _isMenuPaneOpen = false;

    [ObservableProperty]
    private MenuItemTemplate? _selectedMenuItem;

    public static ObservableCollection<MenuItemTemplate> MenuItems { get; } =
    [
        new(typeof(HomePageViewModel), "HomeRegular"),
        new(typeof(BiblePageViewModel), "BookOpenRegular"),
        new(typeof(GuidePageViewModel), "CompassNorthwestRegular"),
        new(typeof(ResourcesPageViewModel), "ClipboardRegular"),
        new(typeof(LibraryPageViewModel), "LibraryRegular"),
        new(typeof(DevPageViewModel), "WindowDevToolsRegular"),
    ];

    partial void OnSelectedMenuItemChanged(MenuItemTemplate? value)
    {
        if (value == null)
        {
            return;
        }

        CurrentPage = GetPageViewModelFromMenuItemTemplate(value);
    }

    private static PageViewModelBase GetPageViewModelFromMenuItemTemplate(MenuItemTemplate value)
    {
        object? viewModel;
        if (Design.IsDesignMode)
        {
            // look for a design-time view model in the same assembly
            var designTimeViewModelType = Assembly
                    .GetAssembly(value.ViewModelType)
                    ?.GetTypes()
                    .FirstOrDefault(t =>
                        t != value.ViewModelType &&
                        value.ViewModelType.IsAssignableFrom(t) &&
                        t.Name.StartsWith("Design"))
                ?? value.ViewModelType;

            viewModel = Activator.CreateInstance(designTimeViewModelType);
        }
        else
        {
            viewModel = Ioc.Default.GetService(value.ViewModelType);
        }

        return viewModel as PageViewModelBase
            ?? throw new InvalidOperationException(
                $"{nameof(MenuItemTemplate)}.{nameof(MenuItemTemplate.ViewModelType)} must derive from {nameof(PageViewModelBase)} but was {value.ViewModelType.Name}.");
    }

    [RelayCommand]
    private void TriggerMenuPane()
    {
        IsMenuPaneOpen = !IsMenuPaneOpen;
    }
}