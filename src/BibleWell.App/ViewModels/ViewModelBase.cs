using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BibleWell.App.ViewModels;
public abstract partial class ViewModelBase : ObservableObject
{
    protected ViewModelBase()
    {
        _errorMessages =
        [
        ];
    }
    [ObservableProperty]
    private ObservableCollection<string>? _errorMessages;
}