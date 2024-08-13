using System.Text.Json;
using Ctoss.Core.Builders.Sorting;
using Ctoss.Models;
using Ctoss.Models.Enums;

namespace Ctoss.Extensions;

public static class EnumerableExtensions
{
    #region Pagination

    public static IEnumerable<T> WithPagination<T>(this IEnumerable<T> query, string jsonPagination)
    {
        var paginationModel = JsonSerializer.Deserialize<Pagination>(jsonPagination);
        return paginationModel is null
            ? query
            : WithPagination(query, paginationModel.StartRow, paginationModel.EndRow);
    }

    public static IEnumerable<T> WithPagination<T>(this IEnumerable<T> query, int startRow, int endRow)
        => query.Skip(startRow - 1).Take(endRow - startRow);

    #endregion

    #region Sorting

    public static IEnumerable<T> WithSorting<T>(this IEnumerable<T> query, string jsonSorting)
        => WithSorting(query, JsonSerializer.Deserialize<List<Sorting>>(jsonSorting));

    public static IEnumerable<T> WithSorting<T>(this IEnumerable<T> query, List<Sorting>? sortings)
    {
        var sortingBuilder = new SortingBuilder();

        if (sortings == null || !sortings.Any())
        {
            return query;
        }

        IOrderedQueryable<T> orderedQuery = null!;
        for (var i = 0; i < sortings.Count; i++)
        {
            var sorting = sortings[i];
            var sortingExpression = sortingBuilder.BuildSortingExpressionV2<T>(sorting);
            if (sortingExpression is null)
                continue;

            if (i == 0)
            {
                orderedQuery = sorting.Order == SortingOrder.Asc
                    ? Queryable.OrderBy(query, (dynamic)sortingExpression)
                    : Queryable.OrderByDescending(query, (dynamic)sortingExpression);
            }
            else
            {
                orderedQuery = sorting.Order == SortingOrder.Asc
                    ? Queryable.ThenBy(orderedQuery, (dynamic)sortingExpression)
                    : Queryable.ThenByDescending(orderedQuery, (dynamic)sortingExpression);
            }
        }

        return orderedQuery ?? query;
    }

    #endregion
}