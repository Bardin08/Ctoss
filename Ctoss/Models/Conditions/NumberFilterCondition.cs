using Ctoss.Models.Enums;

namespace Ctoss.Models.Conditions;

public record NumberFilterCondition : FilterCondition
{
    public decimal? Filter { get; init; }
    public decimal? FilterTo { get; init; }
    public NumberFilterOptions? Type { get; init; }
}
