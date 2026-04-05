using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Changes;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Converters;

internal class ChangeItemConverter : JsonConverter<ChangeItemBase>
{
    public override ChangeItemBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty("action", out var actionJson))
        {
            return null;
        }

        var action = actionJson.Deserialize(TmdbJsonSerializerContext.Default.ChangeAction);
        return action switch
        {
            ChangeAction.Added => jsonDocument.RootElement.Deserialize(TmdbJsonSerializerContext.Default.ChangeItemAdded),
            ChangeAction.Created => jsonDocument.RootElement.Deserialize(TmdbJsonSerializerContext.Default.ChangeItemCreated),
            ChangeAction.Updated => jsonDocument.RootElement.Deserialize(TmdbJsonSerializerContext.Default.ChangeItemUpdated),
            ChangeAction.Deleted => jsonDocument.RootElement.Deserialize(TmdbJsonSerializerContext.Default.ChangeItemDeleted),
            ChangeAction.Destroyed => jsonDocument.RootElement.Deserialize(TmdbJsonSerializerContext.Default.ChangeItemDestroyed),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Write(Utf8JsonWriter writer, ChangeItemBase value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), TmdbJsonSerializerContext.Default);
    }
}
