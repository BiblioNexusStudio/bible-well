using BibleWell.Preferences;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BibleWell.App.Desktop;

public sealed partial class DesktopApp : App
{
    protected override void ConfigurePlatform(ConfigurationBuilder configurationBuilder)
    {
    }

    protected override void RegisterPlatformServices(IServiceCollection services)
    {
        // For now, desktop won't save user preferences.
        services.AddSingleton<IUserPreferencesService, FakeUserPreferencesService>();
    }
}