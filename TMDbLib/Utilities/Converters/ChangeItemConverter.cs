using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Changes;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Polymorphic converter for change items — dispatches on the <c>action</c> discriminator.
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

        Type? targetType;
        if (!element.TryGetProperty("action", out var actionElement))
        {
            // No discriminator: best-effort instantiate the declared type.
            return (ChangeItemBase?)Activator.CreateInstance(typeToConvert);
        }

        var action = actionElement.Deserialize<ChangeAction>();
        targetType = action switch
        {
            ChangeAction.Added => typeof(ChangeItemAdded),
            ChangeAction.Created => typeof(ChangeItemCreated),
            ChangeAction.Updated => typeof(ChangeItemUpdated),
            ChangeAction.Deleted => typeof(ChangeItemDeleted),
            ChangeAction.Destroyed => typeof(ChangeItemDestroyed),
            _ => throw new ArgumentOutOfRangeException(nameof(reader), action, "Unsupported change-item action"),
        };

        return (ChangeItemBase?)element.Deserialize(targetType, options);
    }

    public override void Write(Utf8JsonWriter writer, ChangeItemBase value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
