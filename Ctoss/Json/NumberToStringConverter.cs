using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ctoss.Json;

public class NumberToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetDouble().ToString(),
            JsonTokenType.String => reader.GetString(),
            _ => throw new JsonException("Unexpected token type")
        };
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
