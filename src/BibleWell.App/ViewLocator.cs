using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BibleWell.App.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace BibleWell.App;

public sealed class ViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, Func<Control?>> _locator = [];

    public Control Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock { Text = "Error: No ViewModel provided." };
        }

        var viewModelType = data.GetType();

        // If this is design-time then the view model we received might be a "Design*" view model and the base
        // view model is the one actually registered.
        if (Design.IsDesignMode && viewModelType.BaseType != null && viewModelType.Name.StartsWith("Design"))
        {
            viewModelType = viewModelType.BaseType;
        }

        return _locator.GetValueOrDefault(viewModelType)?.Invoke()
            ?? new TextBlock { Text = $"Error: ViewModel \"{viewModelType.Name}\" not registered." };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }

    public void RegisterViewFactory<TViewModel>(Func<Control?> factory)
        where TViewModel : ViewModelBase
    {
        _locator[typeof(TViewModel)] = factory;
    }

    public void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : Control
    {
        RegisterViewFactory<TViewModel>(Design.IsDesignMode
            ? Activator.CreateInstance<TView>
            : Ioc.Default.GetService<TView>);
    }
}