using System;
using System.Text.Json;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Helpers for writing formatted values to a <see cref="Utf8JsonWriter"/> without
/// allocating an intermediate <see cref="string"/>.
/// </summary>
internal static class Utf8JsonWriterExtensions
{
    private const int MaxStackBufferLength = 128;

    /// <summary>
    /// Writes <paramref name="value"/> as a JSON string using the supplied format, formatting
    /// into a stack buffer instead of allocating a <see cref="string"/>. Falls back to
    /// <see cref="DateTime.ToString(string, IFormatProvider)"/> when the formatted value does
    /// not fit the buffer.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to format.</param>
    /// <param name="format">The format to apply, or <see langword="null"/> for the provider default.</param>
    /// <param name="provider">The format provider used while formatting.</param>
    public static void WriteFormattedStringValue(this Utf8JsonWriter writer, DateTime value, string? format, IFormatProvider provider)
    {
        Span<char> buffer = stackalloc char[MaxStackBufferLength];

        if (value.TryFormat(buffer, out int written, format, provider))
        {
            writer.WriteStringValue(buffer[..written]);
            return;
        }

        writer.WriteStringValue(value.ToString(format, provider));
    }
}
