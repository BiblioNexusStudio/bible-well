using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BibleWell.App.ViewModels;
using BibleWell.App.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BibleWell.App;

// this is the Avalonia equivalent of a Program.cs file
public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // configure dependency injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        // configure Avalonia app main window
        var mainViewModel = serviceProvider.GetRequiredService<OpenFileViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new OpenFileView
            {
                DataContext = mainViewModel
            };
        }

        Ioc.Default.ConfigureServices(serviceProvider);
        base.OnFrameworkInitializationCompleted();
    }

    internal static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<OpenFileViewModel>();
    }
}