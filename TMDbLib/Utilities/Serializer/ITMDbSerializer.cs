using System;
using System.IO;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// Interface for JSON serialization and deserialization in TMDbLib.
/// </summary>
public interface ITMDbSerializer
{
    /// <summary>
    /// Serializes an object to a stream.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="target">The target stream.</param>
    /// <param name="obj">The object to serialize.</param>
    void Serialize<T>(Stream target, T obj);

    /// <summary>
    /// Deserializes a stream to an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="source">The source stream.</param>
    /// <returns>The deserialized instance or null.</returns>
    T? Deserialize<T>(Stream source);

    /// <summary>
    /// Deserializes a stream to an instance of the specified type.
    /// </summary>
    /// <param name="source">The source stream.</param>
    /// <param name="type">The target type.</param>
    /// <returns>The deserialized instance or null.</returns>
    object? Deserialize(Stream source, Type type);
}
