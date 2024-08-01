using Ctoss.Models.Enums;

namespace Ctoss.Models.V2;

public record TextCondition : FilterConditionBase
{
    public TextFilterOptions Type { get; init; }
    public string Filter { get; init; } = null!;
}