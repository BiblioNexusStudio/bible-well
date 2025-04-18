using System.Net.Http.Headers;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using BibleWell.App.Configuration;
using BibleWell.App.ViewModels;
using BibleWell.App.ViewModels.Pages;
using BibleWell.App.Views;
using BibleWell.App.Views.Pages;
using BibleWell.Aquifer;
using BibleWell.Aquifer.Api;
using BibleWell.Aquifer.Data;
using BibleWell.Preferences;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace BibleWell.App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // This method should be implemented in platform-specific projects.
    protected virtual void ConfigurePlatform(ConfigurationBuilder configurationBuilder)
    {
        throw new NotImplementedException();
    }

    // This method should be implemented in platform-specific projects.
    protected virtual void RegisterPlatformServices(IServiceCollection services)
    {
        throw new NotImplementedException();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var config = ConfigureConfiguration();

        var serviceProvider = ConfigureServiceProvider(config);
        Ioc.Default.ConfigureServices(serviceProvider);

        var viewLocator = ConfigureViewLocator();
        DataTemplates.Add(viewLocator);

        var userThemeVariant = serviceProvider.GetRequiredService<IUserPreferencesService>()
            .Get(PreferenceKeys.ThemeVariant, "Default");
        RequestedThemeVariant = new ThemeVariant(userThemeVariant, null);

        // configure Avalonia app main window
        var mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();

        // The desktop application lifetime is only used for the design view.
        // Don't use DI for the view because there is no directly associated ViewModel.
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel,
            };
        }
        // the SingleViewApplicationLifetime is for mobile app configuration (no MainWindow is used)
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var mainView = viewLocator.Build(mainViewModel);
            mainView.DataContext = mainViewModel;
            singleViewPlatform.MainView = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private IConfiguration ConfigureConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        using var globalConfigurationSettingsFileStream = GetAppSettingsFileStream("appsettings.json");
        configurationBuilder.AddJsonStream(globalConfigurationSettingsFileStream);

        using var environmentConfigurationSettingsFileStream = GetAppSettingsFileStream($"appsettings.{environment}.json");
        configurationBuilder.AddJsonStream(environmentConfigurationSettingsFileStream);

        ConfigurePlatform(configurationBuilder);

        return configurationBuilder.Build();

        static Stream GetAppSettingsFileStream(string appSettingsFileName)
        {
            var appSettingsEmbeddedResourceFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.{appSettingsFileName}";
            var configurationStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appSettingsEmbeddedResourceFileName);
            return configurationStream ?? throw new InvalidOperationException($"The embedded resource \"{appSettingsEmbeddedResourceFileName}\" was not found.");
        }
    }

    private ServiceProvider ConfigureServiceProvider(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        var configurationOptions = configuration.Get<ConfigurationOptions>() ??
            throw new InvalidOperationException($"Unable to bind {nameof(ConfigurationOptions)}.");

        services.AddOptions<ConfigurationOptions>().Bind(configuration);

        ConfigureServices(services);
        ConfigureViewModels(services);
        ConfigureViews(services);

        services
            .AddHttpClient<IReadOnlyAquiferService, AquiferApiService>(
                client =>
                {
                    client.BaseAddress = new Uri(configurationOptions.AquiferApiBaseUri);
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(Assembly.GetExecutingAssembly().GetName().Name ?? "", Assembly.GetExecutingAssembly().GetName().Version?.ToString())));
                    client.DefaultRequestHeaders.Add("api-key", configurationOptions.AquiferApiKey);
                })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());

        RegisterPlatformServices(services);

        return services
            .BuildServiceProvider();
    }

    private static AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(0.5), retryCount: 2));
    }

    [Singleton(typeof(AquiferApiService), typeof(IReadOnlyAquiferService))]
    [Singleton(typeof(CachingAquiferService), typeof(ICachingAquiferService))]
    [Singleton(typeof(SqliteAquiferService), typeof(IReadWriteAquiferService))]
    private static partial void ConfigureServices(IServiceCollection services);

    [Singleton(typeof(MainViewModel))]
    [Transient(typeof(BiblePageViewModel))]
    [Transient(typeof(DevPageViewModel))]
    [Transient(typeof(GuidePageViewModel))]
    [Transient(typeof(HomePageViewModel))]
    [Transient(typeof(LibraryPageViewModel))]
    [Transient(typeof(ResourcesPageViewModel))]
    private static partial void ConfigureViewModels(IServiceCollection services);

    [Singleton(typeof(MainView))]
    [Transient(typeof(BiblePageView))]
    [Transient(typeof(DevPageView))]
    [Transient(typeof(GuidePageView))]
    [Transient(typeof(HomePageView))]
    [Transient(typeof(LibraryPageView))]
    [Transient(typeof(ResourcesPageView))]
    private static partial void ConfigureViews(IServiceCollection services);

    // all views must be mapped from the primary view model in this method
    private static ViewLocator ConfigureViewLocator()
    {
        var viewLocator = new ViewLocator();

        viewLocator.RegisterViewFactory<MainViewModel, MainView>();
        viewLocator.RegisterViewFactory<BiblePageViewModel, BiblePageView>();
        viewLocator.RegisterViewFactory<DevPageViewModel, DevPageView>();
        viewLocator.RegisterViewFactory<GuidePageViewModel, GuidePageView>();
        viewLocator.RegisterViewFactory<HomePageViewModel, HomePageView>();
        viewLocator.RegisterViewFactory<LibraryPageViewModel, LibraryPageView>();
        viewLocator.RegisterViewFactory<ResourcesPageViewModel, ResourcesPageView>();

        return viewLocator;
    }
}