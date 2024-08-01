using Ctoss.Models.V2;

namespace Ctoss.Models.AgGrid;

public record AgGridQuery(
    int StartRow,
    int EndRow,
    List<Sorting>? SortModel,
    AgGridFilter? FilterModel
);
