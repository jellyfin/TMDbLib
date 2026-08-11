using System;
using System.Globalization;
using System.Text.Json;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Helpers for parsing values directly out of a <see cref="Utf8JsonReader"/> without
/// allocating an intermediate <see cref="string"/>.
/// </summary>
internal static class Utf8JsonReaderExtensions
{
    private const int MaxStackBufferLength = 256;

    /// <summary>
    /// Parses the current string token using the supplied exact format, transcoding the
    /// value into a stack buffer instead of allocating a <see cref="string"/>.
    /// </summary>
    /// <param name="reader">The reader positioned on the string token to parse.</param>
    /// <param name="format">The exact format the value is expected to match.</param>
    /// <param name="provider">The format provider used while parsing.</param>
    /// <param name="value">The parsed value, or <see langword="default"/> when the token is empty.</param>
    /// <returns>
    /// <see langword="false"/> when the token is an empty string (the caller decides between
    /// <see langword="default"/> and <see langword="null"/>); otherwise <see langword="true"/>.
    /// A non-empty value that does not match the format throws, matching <see cref="DateTime.ParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, IFormatProvider, DateTimeStyles)"/>.
    /// </returns>
    public static bool TryGetDateTimeExact(this ref Utf8JsonReader reader, string format, IFormatProvider provider, out DateTime value)
    {
        int maxLength = GetMaxCharCount(ref reader);

        if (maxLength == 0)
        {
            value = default;
            return false;
        }

        Span<char> buffer = maxLength <= MaxStackBufferLength ? stackalloc char[maxLength] : new char[maxLength];
        int written = reader.CopyString(buffer);

        if (written == 0)
        {
            value = default;
            return false;
        }

        value = DateTime.ParseExact(buffer[..written], format, provider, DateTimeStyles.None);
        return true;
    }

    /// <summary>
    /// Parses the current string token with the lenient <see cref="DateTime.TryParse(ReadOnlySpan{char}, IFormatProvider, DateTimeStyles, out DateTime)"/>
    /// rules, transcoding the value into a stack buffer instead of allocating a <see cref="string"/>.
    /// </summary>
    /// <param name="reader">The reader positioned on the string token to parse.</param>
    /// <param name="provider">The format provider used while parsing.</param>
    /// <param name="value">The parsed value, or <see langword="default"/> when the token is empty or unparseable.</param>
    /// <returns><see langword="true"/> when the token parsed successfully; otherwise <see langword="false"/>.</returns>
    public static bool TryGetDateTimeLenient(this ref Utf8JsonReader reader, IFormatProvider provider, out DateTime value)
    {
        int maxLength = GetMaxCharCount(ref reader);

        if (maxLength == 0)
        {
            value = default;
            return false;
        }

        Span<char> buffer = maxLength <= MaxStackBufferLength ? stackalloc char[maxLength] : new char[maxLength];
        int written = reader.CopyString(buffer);

        return DateTime.TryParse(buffer[..written], provider, DateTimeStyles.None, out value);
    }

    /// <summary>
    /// Parses the current string token as an <see cref="int"/>, transcoding the value into a
    /// stack buffer instead of allocating a <see cref="string"/>.
    /// </summary>
    /// <param name="reader">The reader positioned on the string token to parse.</param>
    /// <param name="value">The parsed value, or <see langword="default"/> when the token is empty or unparseable.</param>
    /// <returns><see langword="true"/> when the token parsed successfully; otherwise <see langword="false"/>.</returns>
    public static bool TryGetInt32FromString(this ref Utf8JsonReader reader, out int value)
    {
        int maxLength = GetMaxCharCount(ref reader);

        if (maxLength == 0)
        {
            value = default;
            return false;
        }

        Span<char> buffer = maxLength <= MaxStackBufferLength ? stackalloc char[maxLength] : new char[maxLength];
        int written = reader.CopyString(buffer);

        return int.TryParse(buffer[..written], NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }

    /// <summary>
    /// Gets an upper bound for the number of characters the current token unescapes to. UTF-8
    /// never encodes a <see cref="char"/> in fewer bytes than it takes to represent it, so the
    /// encoded byte count is always a safe buffer size.
    /// </summary>
    private static int GetMaxCharCount(ref Utf8JsonReader reader)
    {
        return reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;
    }
}
