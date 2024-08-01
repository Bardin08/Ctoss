namespace Ctoss.Models.V2;

public record MultiCondition : FilterConditionBase
{
    public List<FilterModel> FilterModels { get; init; } = null!;
}