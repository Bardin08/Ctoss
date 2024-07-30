namespace Ctoss.Models.AgGrid;

public record AgGridQuery(
    int StartRow,
    int EndRow,
    List<Sorting>? SortModel,
    Dictionary<string, Filter>? FilterModel
);