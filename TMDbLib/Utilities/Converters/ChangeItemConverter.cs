using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using TMDbLib.Objects.Changes;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Polymorphic converter for change items - dispatches on the <c>action</c> discriminator.
/// </summary>
internal class ChangeItemConverter : JsonConverter<ChangeItemBase>
{
    public override ChangeItemBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        if (!element.TryGetProperty("action", out var actionElement))
        {
            return null;
        }

        var action = actionElement.Deserialize((JsonTypeInfo<ChangeAction>)options.GetTypeInfo(typeof(ChangeAction)));
        return action switch
        {
            ChangeAction.Added => (ChangeItemBase?)element.Deserialize(options.GetTypeInfo(typeof(ChangeItemAdded))),
            ChangeAction.Created => (ChangeItemBase?)element.Deserialize(options.GetTypeInfo(typeof(ChangeItemCreated))),
            ChangeAction.Updated => (ChangeItemBase?)element.Deserialize(options.GetTypeInfo(typeof(ChangeItemUpdated))),
            ChangeAction.Deleted => (ChangeItemBase?)element.Deserialize(options.GetTypeInfo(typeof(ChangeItemDeleted))),
            ChangeAction.Destroyed => (ChangeItemBase?)element.Deserialize(options.GetTypeInfo(typeof(ChangeItemDestroyed))),
            _ => throw new ArgumentOutOfRangeException(nameof(reader), action, "Unsupported change-item action"),
        };
    }

    public override void Write(Utf8JsonWriter writer, ChangeItemBase value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var typeInfo = options.GetTypeInfo(value.GetType());
        JsonSerializer.Serialize(writer, value, typeInfo);
    }
}
