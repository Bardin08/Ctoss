using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ctoss.Json;

internal class JsonStringEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        var enumValue = reader.GetString();
        if (Enum.TryParse(enumValue, true, out T value))
        {
            return value;
        }

        throw new JsonException($"Unable to convert \"{enumValue}\" to enum \"{typeof(T)}\".");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
