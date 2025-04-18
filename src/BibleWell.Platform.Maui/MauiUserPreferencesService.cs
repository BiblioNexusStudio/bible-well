using BibleWell.Preferences;

namespace BibleWell.Platform.Maui;

/// <summary>
/// See https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences?view=net-maui-9.0.
/// </summary>
public sealed class MauiUserPreferencesService : IUserPreferencesService
{
    public bool ContainsKey(string key)
    {
        return Microsoft.Maui.Storage.Preferences.Default.ContainsKey(key);
    }

    public void Remove(string key)
    {
        Microsoft.Maui.Storage.Preferences.Default.Remove(key);
    }

    public void Clear()
    {
        Microsoft.Maui.Storage.Preferences.Default.Clear();
    }

    public void Set<T>(string key, T value)
    {
        Microsoft.Maui.Storage.Preferences.Default.Set(key, value);
    }

    public T Get<T>(string key, T defaultValue)
    {
        return Microsoft.Maui.Storage.Preferences.Default.Get(key, defaultValue);
    }
}
