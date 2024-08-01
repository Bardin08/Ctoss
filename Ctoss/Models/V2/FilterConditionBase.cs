using System.Text.Json.Serialization;

namespace Ctoss.Models.V2;

[JsonConverter(typeof(FilterConverter))]
public abstract record FilterConditionBase
{
    public string FilterType { get; init; } = null!;
}