using Ctoss.Core;
using Ctoss.Core.Builders.Sorting;
using Ctoss.Extensions;
using Ctoss.Models;
using Ctoss.Models.AgGrid;

namespace Ctoss;

public class CtossService : ICtossService
{
    private readonly IFilterProvider _filterProvider;
    private readonly ISortingBuilder _sortingBuilder;

    public CtossService(IFilterProvider filterProvider, ISortingBuilder sortingBuilder)
    {
        _filterProvider = filterProvider;
        _sortingBuilder = sortingBuilder;
    }

    public async Task<AgGridQueryResult<T>> ApplyAsync<T>(AgGridQuery query, IEnumerable<T> enumerable)
    {
        var applyFilter = query.FilterModel is { Count: > 0 };
        if (applyFilter)
        {
            var descriptors = query.FilterModel!
                .Select(x => new FilterDescriptor(x.Key, x.Value));
            var predicate = await _filterProvider.GetFilterExpressionAsync<T>(descriptors);

            if (predicate is not null)
                enumerable = enumerable.Where(predicate.Compile());
        }

        var applySorting = query.SortModel is { Count: > 0 };
        if (applySorting)
            enumerable = enumerable.WithSorting(query.SortModel);

        var array = enumerable.ToArray();
        var totalCount = array.Length;
        var paginated = array
            .Skip(query.StartRow)
            .Take(query.EndRow - query.StartRow)
            .ToList();

        return new AgGridQueryResult<T>(paginated, totalCount);
    }
}