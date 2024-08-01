using Ctoss.Models.Enums;

namespace Ctoss.Models.V2;

public record DateCondition : FilterConditionBase
{
    public DateFilterOptions Type { get; init; }
    public string? DateFrom { get; init; }
    public string? DateTo { get; init; }
}