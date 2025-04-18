namespace BibleWell.Preferences;

public interface IUserPreferencesService
{
    /// <summary>
    /// Checks for the existence of a given key.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns><c>true</c> if the key exists in the preferences, otherwise <c>false</c>.</returns>
    bool ContainsKey(string key);

    /// <summary>
    /// Removes a key and its associated value if it exists.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    void Remove(string key);

    /// <summary>
    /// Clears all keys and values.
    /// </summary>
    void Clear();

    /// <summary>
    /// Sets a value for a given key.
    /// </summary>
    /// <typeparam name="T">Type of the object that is stored in this preference.</typeparam>
    /// <param name="key">The key to set the value for.</param>
    /// <param name="value">Value to set.</param>
    void Set<T>(string key, T value);

    /// <summary>
    /// Gets the value for a given key, or the default specified if the key does not exist.
    /// </summary>
    /// <typeparam name="T">The type of the object stored for this preference.</typeparam>
    /// <param name="key">The key to retrieve the value for.</param>
    /// <param name="defaultValue">The default value to return when no existing value for <paramref name="key"/> exists.</param>
    /// <returns>Value for the given key, or the value in <paramref name="defaultValue"/> if it does not exist.</returns>
    T Get<T>(string key, T defaultValue);
}