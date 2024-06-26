namespace Ctoss.Models.Conditions;

public record TextFilterCondition : FilterCondition
{
    public string? Filter { get; init; }
    public Enums.TextFilterOptions Type { get; init; }
}
