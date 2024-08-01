namespace Ctoss.Models.V2;

public record SetCondition : FilterConditionBase
{
    public List<string> Values { get; init; } = null!;
}