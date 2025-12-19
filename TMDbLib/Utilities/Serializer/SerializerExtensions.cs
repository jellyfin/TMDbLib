using System;
using System.IO;
using System.Text;

namespace TMDbLib.Utilities.Serializer;

/// <summary>
/// Extension methods for <see cref="ITMDbSerializer"/>.
/// </summary>
public static class SerializerExtensions
{
    /// <summary>
    /// Serializes an object to a stream.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="serializer">The serializer instance.</param>
    /// <param name="target">The target stream to write to.</param>
    /// <param name="object">The object to serialize.</param>
    public static void Serialize<T>(this ITMDbSerializer serializer, Stream target, T @object)
    {
        serializer.Serialize(target, @object, typeof(T));
    }

    /// <summary>
    /// Serializes an object to a byte array.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="serializer">The serializer instance.</param>
    /// <param name="object">The object to serialize.</param>
    /// <returns>A byte array containing the serialized object.</returns>
    public static byte[] SerializeToBytes<T>(this ITMDbSerializer serializer, T @object)
    {
        using MemoryStream ms = new MemoryStream();

        serializer.Serialize(ms, @object, typeof(T));

        return ms.ToArray();
    }

    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="serializer">The serializer instance.</param>
    /// <param name="object">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public static string SerializeToString<T>(this ITMDbSerializer serializer, T @object)
    {
        using MemoryStream ms = new MemoryStream();

        serializer.Serialize(ms, @object, typeof(T));

        ms.Seek(0, SeekOrigin.Begin);

        using StreamReader sr = new StreamReader(ms, Encoding.UTF8);

        return sr.ReadToEnd();
    }

    /// <summary>
    /// Deserializes an object from a stream.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="serializer">The serializer instance.</param>
    /// <param name="source">The source stream to read from.</param>
    /// <returns>The deserialized object.</returns>
    public static T Deserialize<T>(this ITMDbSerializer serializer, Stream source)
    {
        return (T)serializer.Deserialize(source, typeof(T));
    }

    /// <summary>
    /// Deserializes an object from a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="serializer">The serializer instance.</param>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    public static T DeserializeFromString<T>(this ITMDbSerializer serializer, string json)
    {
        // TODO: Better method
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        using MemoryStream ms = new MemoryStream(bytes);

        return serializer.Deserialize<T>(ms);
    }

    /// <summary>
    /// Deserializes an object from a JSON string.
    /// </summary>
    /// <param name="serializer">The serializer instance.</param>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    public static object DeserializeFromString(this ITMDbSerializer serializer, string json, Type type)
    {
        // TODO: Better method
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        using MemoryStream ms = new MemoryStream(bytes);

        return serializer.Deserialize(ms, type);
    }
}
