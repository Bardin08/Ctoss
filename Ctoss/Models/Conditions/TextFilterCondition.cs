namespace Ctoss.Models.Conditions;

public class TextFilterCondition : FilterCondition
{
    public string? Filter { get; set; }
    public Enums.TextFilterOptions Type { get; set; }
}
