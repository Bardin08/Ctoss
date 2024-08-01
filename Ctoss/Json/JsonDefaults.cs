using System.Text.Json;
using System.Text.Json.Serialization;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;

namespace Ctoss.Json;

public static class CtossJsonDefaults
{
    public static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        Converters =
        {
            new FilterConverter(),
            new NumberToStringConverter(),
            new JsonStringEnumConverter<Operator>(),
            new JsonStringEnumConverter<TextFilterOptions>(),
            new JsonStringEnumConverter<DateFilterOptions>(),
            new JsonStringEnumConverter<NumberFilterOptions>(),
            new JsonStringEnumConverter<SortingOrder>()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        NumberHandling = JsonNumberHandling.WriteAsString
    };
}
