namespace Ctoss.Models.AgGrid;

public record AgGridQueryResult<TModel>(
    List<TModel> Rows,
    int TotalCount
);