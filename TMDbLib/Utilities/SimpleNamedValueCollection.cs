using System;
using System.Collections;
using System.Collections.Generic;

namespace TMDbLib.Utilities;

/// <summary>
/// A simple collection of named string values with case-insensitive key matching.
/// </summary>
public class SimpleNamedValueCollection : IEnumerable<KeyValuePair<string, string>>
{
    private readonly List<KeyValuePair<string, string>> _list;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNamedValueCollection"/> class.
    /// </summary>
    public SimpleNamedValueCollection()
    {
        _list = new List<KeyValuePair<string, string>>();
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="index">The key of the value to get or set.</param>
    /// <returns>The value associated with the specified key.</returns>
    public string this[string index]
    {
        get => Get(index); set => Add(index, value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (KeyValuePair<string, string> pair in _list)
        {
            yield return pair;
        }
    }

    /// <summary>
    /// Adds or updates a key-value pair in the collection.
    /// </summary>
    /// <param name="key">The key of the element to add or update.</param>
    /// <param name="value">The value of the element to add or update.</param>
    public void Add(string key, string value)
    {
        Remove(key);

        _list.Add(new KeyValuePair<string, string>(key, value));
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="default">The default value to return if the key is not found.</param>
    /// <returns>The value associated with the specified key, or the default value if not found.</returns>
    public string Get(string key, string @default = null)
    {
        foreach (KeyValuePair<string, string> pair in _list)
        {
            if (pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                return pair.Value;
            }
        }

        return @default;
    }

    /// <summary>
    /// Removes the element with the specified key from the collection.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>True if the element was successfully removed; otherwise, false.</returns>
    public bool Remove(string key)
    {
        return _list.RemoveAll(s => s.Key.Equals(key, StringComparison.OrdinalIgnoreCase)) > 0;
    }
}
