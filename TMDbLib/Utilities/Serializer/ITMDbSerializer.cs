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
    /// <param name="target">The target stream to write to.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="type">The type of the object.</param>
    void Serialize(Stream target, object obj, Type type);

    /// <summary>
    /// Deserializes an object from a stream.
    /// </summary>
    /// <param name="source">The source stream to read from.</param>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    object Deserialize(Stream source, Type type);
}
