using Ctoss.Models.Enums;

namespace Ctoss.Models.V2;

public record NumberCondition : FilterConditionBase
{
    public NumberFilterOptions Type { get; init; }
    public string? Filter { get; init; }
    public string? FilterTo { get; init; }
}