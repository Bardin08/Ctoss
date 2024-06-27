using Ctoss.Models.Enums;

namespace Ctoss.Models.Conditions;

public record NumberFilterCondition : FilterCondition
{
    public string? Filter { get; init; }
    public string? FilterTo { get; init; }
    public NumberFilterOptions? Type { get; init; }
}
