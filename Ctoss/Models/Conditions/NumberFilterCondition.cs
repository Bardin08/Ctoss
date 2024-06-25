using Ctoss.Models.Enums;

namespace Ctoss.Models.Conditions;

public class NumberFilterCondition : FilterCondition
{
    public decimal? Filter { get; set; }
    public decimal? FilterTo { get; set; }
    public NumberFilterOptions? Type { get; set; }
}
