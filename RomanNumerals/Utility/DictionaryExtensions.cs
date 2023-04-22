namespace RomanNumerals.Utility;

using System;
using System.Collections.Generic;

/// <summary>
/// Extensions to dictionary
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Tries to get a value from dictionary or creates a new one if necessary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="ctor"></param>
    /// <returns></returns>
    public static TValue TryGetOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> ctor)
    {
        if (dictionary.TryGetValue(key, out var value))
            return value;
        dictionary[key] = value = ctor();
        return value;
    }

    /// <summary>
    /// Tries to get a value from dictionary or creates a new one if necessary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TValue TryGetOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TValue : new()
    {
        return TryGetOrAddNew(dictionary, key, () => new TValue());
    }

    /// <summary>
    /// Tries to get a value from dictionary or creates a new one if necessary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TValue TryGetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary.TryGetValue(key, out var value))
            return value;
        return default;
    }
}
