using System.Text.Json;
using Ctoss.Builders.Filters;
using Ctoss.Builders.Sorting;
using Ctoss.Json;
using Ctoss.Models;
using Ctoss.Models.Enums;

namespace Ctoss.Extensions;

public static class QueryableExtensions
{
    #region Pagination

    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, string jsonPagination)
    {
        var paginationModel = JsonSerializer.Deserialize<Pagination>(jsonPagination);
        return paginationModel is null
            ? query
            : WithPagination(query, paginationModel.StartRow, paginationModel.EndRow);
    }

    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, int startRow, int endRow)
        => query.Skip(startRow - 1).Take(endRow - startRow);

    #endregion

    #region Sorting

    public static IQueryable<T> WithSorting<T>(this IQueryable<T> query, string jsonSorting)
        => WithSorting(query, JsonSerializer.Deserialize<List<Sorting>>(jsonSorting));

    public static IQueryable<T> WithSorting<T>(this IQueryable<T> query, List<Sorting>? sortings)
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

    #region Filtering

    public static IQueryable<T> WithFilter<T>(
        this IQueryable<T> query, string jsonFilter) =>
        query.WithFilter(
            JsonSerializer.Deserialize<Dictionary<string, Filter>>(
                jsonFilter, CtossJsonDefaults.DefaultJsonOptions)
        );

    public static IQueryable<T> WithFilter<T>(
        this IQueryable<T> query, string propertyName, Filter? filter) =>
        filter is null
            ? query
            : WithFilter(query, new Dictionary<string, Filter> { { propertyName, filter } });

    public static IQueryable<T> WithFilter<T>(
        this IQueryable<T> query, Dictionary<string, Filter>? filters)
    {
        if (filters is null || !filters.Any())
            return query;

        var filterBuilder = new FilterBuilder();
        var predicate = filterBuilder.GetExpression<T>(filters);

        if (predicate is null)
            throw new ArgumentException("Invalid filter");

        return query.Where(predicate);
    }

    #endregion
}