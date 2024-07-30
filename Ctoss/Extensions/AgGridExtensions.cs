using Ctoss.Models.AgGrid;

namespace Ctoss.Extensions;

public static class AgGridExtensions
{
    public static AgGridQueryResult<T> Apply<T>(IQueryable<T> all, AgGridQuery query)
    {
        var applyFilter = query.FilterModel is { Count: > 0 };
        if (applyFilter)
            all = all.WithFilter(query.FilterModel!);

        var applySorting = query.SortModel is { Count: > 0 };
        if (applySorting)
            all = all.WithSorting(query.SortModel);

        var totalCount = all.Count();
        var paginated = all
            .Skip(query.StartRow - 1)
            .Take(query.EndRow - query.StartRow)
            .ToList();

        return new AgGridQueryResult<T>(paginated, totalCount);
    }
}