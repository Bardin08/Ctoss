using Ctoss.Models.Enums;

namespace Ctoss.Models.Conditions;

public record DateFilterCondition : FilterCondition
{
    public string? DateFrom { get; init; }
    public string? DateTo { get; init; }
    public DateFilterOptions? Type { get; init; }
}
