using Ctoss.Models.V2;

namespace Ctoss.Models.AgGrid;

public record AgGridQuery
{
    public List<Sorting>? SortModel { get; init; }
    public Dictionary<string, FilterModel>? FilterModel { get; init; }
    public int StartRow { get; init; } = 0;
    public int EndRow { get; init; } = 100;
}