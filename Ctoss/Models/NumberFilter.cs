using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;

namespace Ctoss.Models;

public class NumberFilter
{
    public string FilterType { get; set; } = null!;
    public Operator Operator { get; set; }
    public FilterCondition? Condition1 { get; set; }
    public FilterCondition? Condition2 { get; set; }
    public List<FilterCondition>? Conditions { get; set; }
}
