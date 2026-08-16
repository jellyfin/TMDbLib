using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using TMDbLib.Client;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Verifies the source-generated enum maps against the <see cref="EnumValueAttribute"/>
/// declarations they are generated from.
/// </summary>
/// <remarks>
/// TMDbLib is reflection-free so it can be trimmed and AOT-compiled; these tests read the
/// attributes reflectively (which is fine in a JIT-compiled test run) and assert the generated
/// switches agree. If someone adds an enum member and the generator misses it, this fails.
/// </remarks>
public class SourceGeneratedEnumMapTests : TestBase
{
    /// <summary>
    /// Gets every enum in TMDbLib marked with <see cref="TolerantEnumAttribute"/>.
    /// </summary>
    public static TheoryData<Type> TolerantEnums
    {
        get
        {
            var data = new TheoryData<Type>();

            foreach (var type in typeof(TMDbClient).Assembly.GetTypes()
                         .Where(t => t.IsEnum && t.GetCustomAttribute<TolerantEnumAttribute>() is not null)
                         .OrderBy(t => t.FullName, StringComparer.Ordinal))
            {
                data.Add(type);
            }

            return data;
        }
    }

    /// <summary>
    /// Every enum marked for generation must actually have gotten a GetDescription overload.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    [Theory]
    [MemberData(nameof(TolerantEnums))]
    public void EveryTolerantEnumHasGeneratedDescription(Type enumType)
    {
        Assert.NotNull(FindGetDescription(enumType));
    }

    /// <summary>
    /// The generated GetDescription must return exactly what the attributes declare.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    [Theory]
    [MemberData(nameof(TolerantEnums))]
    public void GeneratedDescriptionMatchesAttributes(Type enumType)
    {
        var method = FindGetDescription(enumType);
        Assert.NotNull(method);

        foreach (var value in Enum.GetValues(enumType))
        {
            // The old reflection cache fell back to the member name when there was no
            // [EnumValue], or when the attribute carried a null value.
            var expected = EnumValueLookup.GetString(value) ?? value.ToString();
            var actual = (string?)method!.Invoke(null, [value]);

            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// Every declared member must survive a JSON round trip through the generated converter.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    [Theory]
    [MemberData(nameof(TolerantEnums))]
    public void GeneratedConverterRoundTripsEveryMember(Type enumType)
    {
        var options = new JsonSerializerOptions();
        TmdbEnumConverters.RegisterAll(options);

        // Enums declared with GenerateJsonConverter = false have no converter by design.
        if (!options.Converters.Any(c => c.CanConvert(enumType)))
        {
            return;
        }

        var expectedStrings = new Dictionary<string, object>(StringComparer.Ordinal);

        foreach (var value in Enum.GetValues(enumType))
        {
            var json = JsonSerializer.Serialize(value, enumType, options);
            var expected = EnumValueLookup.GetString(value) ?? value.ToString();

            Assert.Equal($"\"{expected}\"", json);

            // Aliases share a value, so only round-trip the first member that produced a string.
            if (expectedStrings.TryAdd(json, value!))
            {
                Assert.Equal(value, JsonSerializer.Deserialize(json, enumType, options));
            }
        }
    }

    /// <summary>
    /// Unrecognised values must fall back rather than throw, which is why TMDb needs a
    /// "tolerant" converter in the first place.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    [Theory]
    [MemberData(nameof(TolerantEnums))]
    public void GeneratedConverterToleratesUnknownValues(Type enumType)
    {
        var options = new JsonSerializerOptions();
        TmdbEnumConverters.RegisterAll(options);

        if (!options.Converters.Any(c => c.CanConvert(enumType)))
        {
            return;
        }

        var names = Enum.GetNames(enumType);
        var fallbackName = names.FirstOrDefault(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase))
                           ?? names[0];
        var fallback = Enum.Parse(enumType, fallbackName);

        Assert.Equal(fallback, JsonSerializer.Deserialize("\"no-such-value-here\"", enumType, options));
        Assert.Equal(fallback, JsonSerializer.Deserialize("999999", enumType, options));
    }

    private static MethodInfo? FindGetDescription(Type enumType)
    {
        return typeof(EnumExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(m => m.Name == nameof(EnumExtensions.GetDescription)
                                 && m.GetParameters() is [var p]
                                 && p.ParameterType == enumType);
    }
}
