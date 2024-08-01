using System.Text.Json.Serialization;
using Ctoss.Json;
using Ctoss.Models.Enums;

namespace Ctoss.Models.V2;

public record FilterModel
{
    public string FilterType { get; init; } = null!;
    public string Type { get; init; } = null!;
    public Operator? Operator { get; init; }
    public List<FilterConditionBase>? Conditions { get; init; }

    // This is done to support plain filters. Most of these properties will be empty
    [JsonConverter(typeof(NumberToStringConverter))]
    public string? Filter { get; set; }
    [JsonConverter(typeof(NumberToStringConverter))]
    public string? FilterTo { get; set; }
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public List<string> Values { get; init; } = null!;
}