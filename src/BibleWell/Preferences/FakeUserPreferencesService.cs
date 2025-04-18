namespace BibleWell.Preferences;

/// <summary>
/// A fake implementation of <see cref="IUserPreferencesService"/> for design-time use.
/// </summary>
public class FakeUserPreferencesService : IUserPreferencesService
{
    public bool ContainsKey(string key)
    {
        return false;
    }

    public void Remove(string key)
    {
    }

    public void Clear()
    {
    }

    public void Set<T>(string key, T value)
    {
    }

    public T Get<T>(string key, T defaultValue)
    {
        return defaultValue;
    }
}