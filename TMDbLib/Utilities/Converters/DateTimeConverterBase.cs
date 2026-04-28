using System.Globalization;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Base class for date/time converters.
/// </summary>
/// <typeparam name="T">The type being converted.</typeparam>
internal abstract class DateTimeConverterBase<T> : JsonConverter<T>
{
    /// <summary>
    /// Gets or sets the date time styles used when converting a date to and from JSON.
    /// </summary>
    /// <value>The date time styles used when converting a date to and from JSON.</value>
    public DateTimeStyles DatetimeStyles { get; set; } = DateTimeStyles.RoundtripKind;

    /// <summary>
    /// Gets or sets the date time format used when converting a date to and from JSON.
    /// </summary>
    public string? DatetimeFormat
    {
        get => field ?? string.Empty;
        set => field = string.IsNullOrEmpty(value) ? null : value;
    }

    /// <summary>
    /// Gets or sets the culture used when converting a date to and from JSON.
    /// </summary>
    public CultureInfo CultureInfo
    {
        get => field ?? CultureInfo.CurrentCulture;
        set;
    }
}
