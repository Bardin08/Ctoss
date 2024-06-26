using System.Text.Json;
using Ctoss.Models.Enums;

namespace Ctoss.Json;

public static class CtossJsonDefaults
{
    public static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        Converters =
        {
            new FilterConditionConverter(),
            new JsonStringEnumConverter<Operator>(),
            new JsonStringEnumConverter<TextFilterOptions>(),
            new JsonStringEnumConverter<DateFilterOptions>(),
            new JsonStringEnumConverter<NumberFilterOptions>()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
