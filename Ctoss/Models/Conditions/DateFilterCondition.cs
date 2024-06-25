using Ctoss.Models.Enums;

namespace Ctoss.Models.Conditions;

public class DateFilterCondition : FilterCondition
{
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public DateFilterOptions? Type { get; set; }
}
