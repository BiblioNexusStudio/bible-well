using BibleWell.Platform.Maui;
using BibleWell.Preferences;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BibleWell.App.Android;

public sealed class AndroidApp : App
{
    protected override void ConfigurePlatform(ConfigurationBuilder configurationBuilder)
    {
    }

    protected override void RegisterPlatformServices(IServiceCollection services)
    {
        services.AddSingleton<IUserPreferencesService, MauiUserPreferencesService>();
    }
}