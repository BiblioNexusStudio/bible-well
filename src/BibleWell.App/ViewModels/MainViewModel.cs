using CommunityToolkit.Mvvm.ComponentModel;

namespace BibleWell.App.ViewModels;
public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}