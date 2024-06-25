using System.Text.Json;
using System.Text.Json.Serialization;
using Ctoss.Models.Conditions;

namespace Ctoss.Json;

public class FilterConditionConverter : JsonConverter<FilterCondition>
{
    public override FilterCondition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var filterType = root.GetProperty("filterType").GetString()
                         ?? throw new ArgumentException("filterType is required");

        return (filterType switch
        {
            "text" => JsonSerializer.Deserialize<TextFilterCondition>(root.GetRawText(), options),
            "number" => JsonSerializer.Deserialize<NumberFilterCondition>(root.GetRawText(), options),
            "date" => JsonSerializer.Deserialize<DateFilterCondition>(root.GetRawText(), options),
            _ => throw new NotSupportedException($"FilterType {filterType} is not supported")
        })!;
    }

    public override void Write(Utf8JsonWriter writer, FilterCondition value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}
