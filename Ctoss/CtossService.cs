using System.Diagnostics.CodeAnalysis;
using Ctoss.Core;
using Ctoss.Models;
using Ctoss.Models.AgGrid;
using Ctoss.Models.Enums;

namespace Ctoss;

public class CtossService : ICtossService
{
    private readonly IFilterProvider _filterProvider;
    private readonly ISortingProvider _sortingProvider;

    public CtossService(IFilterProvider filterProvider, ISortingProvider sortingProvider)
    {
        _filterProvider = filterProvider;
        _sortingProvider = sortingProvider;
    }

    public AgGridQueryResult<TModel> Apply<TModel>(AgGridQuery query, IEnumerable<TModel> enumerable)
    {
        var applyFilter = query.FilterModel is { Count: > 0 };
        if (applyFilter)
        {
            var descriptors = query.FilterModel!
                .Select(x => new FilterDescriptor(x.Key, x.Value));
            var predicate = _filterProvider.GetFilterExpression<TModel>(descriptors);

            if (predicate is not null)
                enumerable = enumerable.Where(predicate.Compile());
        }

        var applySorting = query.SortModel is { Count: > 0 };
        if (applySorting)
            enumerable = ApplySorting(query, enumerable);

        var array = enumerable.ToArray();
        var totalCount = array.Length;
        var paginated = array
            .Skip(query.StartRow)
            .Take(query.EndRow - query.StartRow)
            .ToList();

        return new AgGridQueryResult<TModel>(paginated, totalCount);
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private IEnumerable<TModel> ApplySorting<TModel>(AgGridQuery query, IEnumerable<TModel> enumerable)
    {
        IOrderedEnumerable<TModel>? orderedEnumerable = null;

        foreach (var sorting in query.SortModel!)
        {
            var sortingExpression = _sortingProvider.GetSortingExpression<TModel>(sorting);
            if (sortingExpression == null)
                continue;

            if (orderedEnumerable == null)
            {
                orderedEnumerable = sorting.Order == SortingOrder.Asc
                    ? enumerable.OrderBy(sortingExpression.Compile())
                    : enumerable.OrderByDescending(sortingExpression.Compile());
            }
            else
            {
                orderedEnumerable = sorting.Order == SortingOrder.Asc
                    ? orderedEnumerable.ThenBy(sortingExpression.Compile())
                    : orderedEnumerable.ThenByDescending(sortingExpression.Compile());
            }
        }

        return orderedEnumerable ?? enumerable;
    }
}