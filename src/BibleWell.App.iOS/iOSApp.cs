using BibleWell.Platform.Maui;
using BibleWell.Preferences;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BibleWell.App.iOS;

#pragma warning disable IDE1006 // Naming Styles: Allow "iOS" prefix
// ReSharper disable once InconsistentNaming
public sealed class iOSApp : App
#pragma warning restore IDE1006 // Naming Styles
{
    protected override void ConfigurePlatform(ConfigurationBuilder configurationBuilder)
    {
    }

    protected override void RegisterPlatformServices(IServiceCollection services)
    {
        services.AddSingleton<IUserPreferencesService, MauiUserPreferencesService>();
    }
}