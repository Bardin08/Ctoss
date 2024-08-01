namespace Ctoss.Models.V2;

public record AgGridFilter
{
    public Dictionary<string, FilterModel> Filters { get; init; } = null!;
}