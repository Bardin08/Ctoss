using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ctoss.Models.V2;

public class FilterConverter : JsonConverter<FilterConditionBase?>
{
    public override FilterConditionBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var filterType = root.GetProperty("filterType").GetString();

        return filterType switch
        {
            "text" => JsonSerializer.Deserialize<TextCondition>(root.GetRawText(), options),
            "number" => JsonSerializer.Deserialize<NumberCondition>(root.GetRawText(), options),
            "date" => JsonSerializer.Deserialize<DateCondition>(root.GetRawText(), options),
            "set" => JsonSerializer.Deserialize<SetCondition>(root.GetRawText(), options),
            "multi" => JsonSerializer.Deserialize<MultiCondition>(root.GetRawText(), options),
            _ => throw new NotSupportedException($"Filter type '{filterType}' is not supported")
        };
    }

    public override void Write(Utf8JsonWriter writer, FilterConditionBase? value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}