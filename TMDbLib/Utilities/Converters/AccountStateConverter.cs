using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Normalises the TMDb <c>rated</c> field (which is either <c>false</c> or
/// <c>{ "value": n }</c>) into a <c>rating</c> property on the C# object.
/// </summary>
internal class AccountStateConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(AccountState)
            || typeToConvert == typeof(TvAccountState)
            || typeToConvert == typeof(TvEpisodeAccountState)
            || typeToConvert == typeof(TvEpisodeAccountStateWithNumber);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(AccountStateConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}
