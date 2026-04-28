#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> to and from the ISO 8601 date format (e.g. <c>"2008-04-12T12:53Z"</c>).
    /// </summary>
    internal class IsoDateTimeConverterFactory : JsonConverterFactory
    {
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private readonly IsoDateTimeConverter _isoDateTimeConverter = new();

        private readonly IsoDateTimeNullableConverter _isoDateTimeNullableConverter = new();

        private readonly IsoDateTimeOffsetConverter _isoDateTimeOffsetConverter = new();

        private readonly IsoDateTimeOffsetNullableConverter _isoDateTimeOffsetNullableConverter = new();

        /// <summary>
        /// Gets or sets the date time styles used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time styles used when converting a date to and from JSON.</value>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace should be followed by blank line", Justification = "There should be no warning here.")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1500:Braces for multi-line statements should not share line", Justification = "There should be no warning here.")]
        public DateTimeStyles DateTimeStyles
        {
            get;
            set
            {
                field = value;
                UpdateConverters();
            }
        } = DateTimeStyles.RoundtripKind;

        /// <summary>
        /// Gets or sets the date time format used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time format used when converting a date to and from JSON.</value>
        public string? DateTimeFormat
        {
            get => field ?? string.Empty;
            set
            {
                field = string.IsNullOrEmpty(value) ? null : value;
                UpdateConverters();
            }
        }

        /// <summary>
        /// Gets or sets the culture used when converting a date to and from JSON.
        /// </summary>
        /// <value>The culture used when converting a date to and from JSON.</value>
        public CultureInfo Culture
        {
            get => field ?? CultureInfo.CurrentCulture;
            set
            {
                field = value;
                UpdateConverters();
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?) ||
                   typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(DateTime))
            {
                return _isoDateTimeConverter;
            }

            if (typeToConvert == typeof(DateTime?))
            {
                return _isoDateTimeNullableConverter;
            }

            if (typeToConvert == typeof(DateTimeOffset))
            {
                return _isoDateTimeOffsetConverter;
            }

            if (typeToConvert == typeof(DateTimeOffset?))
            {
                return _isoDateTimeOffsetNullableConverter;
            }

            return null;
        }

        private void UpdateConverters()
        {
            _isoDateTimeConverter.DatetimeStyles = DateTimeStyles;
            _isoDateTimeConverter.DatetimeFormat = DateTimeFormat;
            _isoDateTimeConverter.CultureInfo = Culture;

            _isoDateTimeNullableConverter.DatetimeStyles = DateTimeStyles;
            _isoDateTimeNullableConverter.DatetimeFormat = DateTimeFormat;
            _isoDateTimeNullableConverter.CultureInfo = Culture;

            _isoDateTimeOffsetConverter.DatetimeStyles = DateTimeStyles;
            _isoDateTimeOffsetConverter.DatetimeFormat = DateTimeFormat;
            _isoDateTimeOffsetConverter.CultureInfo = Culture;

            _isoDateTimeOffsetNullableConverter.DatetimeStyles = DateTimeStyles;
            _isoDateTimeOffsetNullableConverter.DatetimeFormat = DateTimeFormat;
            _isoDateTimeOffsetNullableConverter.CultureInfo = Culture;
        }

        private sealed class IsoDateTimeConverter : DateTimeConverterBase<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    throw new JsonException($"Cannot converter null value to {nameof(DateTime)}");
                }

                if (reader.TryGetDateTime(out DateTime dateTime))
                {
                    return dateTime;
                }

                if (reader.TryGetDateTimeOffset(out DateTimeOffset offset))
                {
                    return offset.DateTime;
                }

                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token parsing date. Expected String, got {nameof(DateTime)}.");
                }

                var str = reader.GetString();
                if (string.IsNullOrEmpty(str))
                {
                    throw new JsonException($"Cannot convert null value to {nameof(DateTime)}");
                }

                if (!string.IsNullOrEmpty(DatetimeFormat))
                {
                    return DateTime.ParseExact(str, DatetimeFormat, CultureInfo, DatetimeStyles);
                }

                return DateTime.Parse(str, CultureInfo, DatetimeStyles);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                if ((DatetimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (DatetimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    value = value.ToUniversalTime();
                }

                writer.WriteStringValue(value.ToString(DatetimeFormat ?? DefaultDateTimeFormat, CultureInfo));
            }
        }

        private sealed class IsoDateTimeNullableConverter : DateTimeConverterBase<DateTime?>
        {
            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }

                if (reader.TryGetDateTime(out DateTime dateTime))
                {
                    return dateTime;
                }

                if (reader.TryGetDateTimeOffset(out DateTimeOffset offset))
                {
                    return offset.DateTime;
                }

                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token parsing date. Expected String, got {nameof(DateTime)}.");
                }

                var str = reader.GetString();
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(DatetimeFormat))
                {
                    return DateTime.ParseExact(str, DatetimeFormat, CultureInfo, DatetimeStyles);
                }

                return DateTime.Parse(str, CultureInfo, DatetimeStyles);
            }

            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                if (!value.HasValue)
                {
                    writer.WriteNullValue();
                    return;
                }

                if ((DatetimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (DatetimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    value = value.Value.ToUniversalTime();
                }

                writer.WriteStringValue(value.Value.ToString(DatetimeFormat ?? DefaultDateTimeFormat, CultureInfo));
            }
        }

        private sealed class IsoDateTimeOffsetConverter : DateTimeConverterBase<DateTimeOffset>
        {
            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    throw new JsonException($"Cannot converter null value to {nameof(DateTime)}");
                }

                if (reader.TryGetDateTimeOffset(out DateTimeOffset offset))
                {
                    return offset;
                }

                if (reader.TryGetDateTime(out DateTime dateTime))
                {
                    return dateTime;
                }

                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token parsing date. Expected String, got {nameof(DateTime)}.");
                }

                var str = reader.GetString();
                if (string.IsNullOrEmpty(str))
                {
                    throw new JsonException($"Cannot convert null value to {nameof(DateTime)}");
                }

                if (!string.IsNullOrEmpty(DatetimeFormat))
                {
                    return DateTimeOffset.ParseExact(str, DatetimeFormat, CultureInfo, DatetimeStyles);
                }

                return DateTimeOffset.Parse(str, CultureInfo, DatetimeStyles);
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                if ((DatetimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (DatetimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    value = value.ToUniversalTime();
                }

                writer.WriteStringValue(value.ToString(DatetimeFormat ?? DefaultDateTimeFormat, CultureInfo));
            }
        }

        private sealed class IsoDateTimeOffsetNullableConverter : DateTimeConverterBase<DateTimeOffset?>
        {
            public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }

                if (reader.TryGetDateTimeOffset(out DateTimeOffset offset))
                {
                    return offset;
                }

                if (reader.TryGetDateTime(out DateTime dateTime))
                {
                    return dateTime;
                }

                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token parsing date. Expected String, got {nameof(DateTime)}.");
                }

                var str = reader.GetString();
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(DatetimeFormat))
                {
                    return DateTimeOffset.ParseExact(str, DatetimeFormat, CultureInfo, DatetimeStyles);
                }

                return DateTimeOffset.Parse(str, CultureInfo, DatetimeStyles);
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
            {
                if (!value.HasValue)
                {
                    writer.WriteNullValue();
                    return;
                }

                if ((DatetimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (DatetimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    value = value.Value.ToUniversalTime();
                }

                writer.WriteStringValue(value.Value.ToString(DatetimeFormat ?? DefaultDateTimeFormat, CultureInfo));
            }
        }
    }
}
