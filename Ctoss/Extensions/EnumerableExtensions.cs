using System.Text.Json;
using Ctoss.Builders.Filters;
using Ctoss.Builders.Sorting;
using Ctoss.Models;
using Ctoss.Models.Enums;

namespace Ctoss.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WithPagination<T>(this IEnumerable<T> query, int page, int pageSize)
        => query.Skip((page - 1) * pageSize).Take(pageSize);

    public static IEnumerable<T> WithPagination<T>(this IEnumerable<T> query, Pagination pagination)
        => WithPagination(query, pagination.Page, pagination.PageSize);

    public static IEnumerable<T> WithPagination<T>(this IEnumerable<T> query, string jsonPagination)
        => WithPagination(query, JsonSerializer.Deserialize<Pagination>(jsonPagination)
                                 ?? throw new ArgumentException("Invalid pagination"));

    public static IEnumerable<T> WithSorting<T>(this IEnumerable<T> query, string jsonSorting)
        => WithSorting(query, JsonSerializer.Deserialize<List<Sorting>>(jsonSorting));

    public static IEnumerable<T> WithSorting<T>(this IEnumerable<T> enumerable, List<Sorting>? sortings)
    {
        var sortingBuilder = new SortingBuilder();

        if (sortings == null || !sortings.Any())
        {
            return enumerable;
        }

        IOrderedEnumerable<T> orderedEnumerable = null!;
        for (var i = 0; i < sortings.Count; i++)
        {
            var sorting = sortings[i];
            var sortingExpression = sortingBuilder.BuildSortingExpressionV2<T>(sorting);
            if (sortingExpression is null)
                continue;

            var sortingFunc = sortingExpression.Compile();
            if (i == 0)
            {
                orderedEnumerable = sorting.Order == SortingOrder.Asc
                    ? Enumerable.OrderBy(enumerable, (dynamic)sortingFunc)
                    : Queryable.OrderByDescending(enumerable, (dynamic)sortingFunc);
            }
            else
            {
                orderedEnumerable = sorting.Order == SortingOrder.Asc
                    ? Queryable.ThenBy(orderedEnumerable, (dynamic)sortingFunc)
                    : Queryable.ThenByDescending(orderedEnumerable, (dynamic)sortingFunc);
            }
        }

        return orderedEnumerable ?? enumerable;
    }

    public static IEnumerable<T> WithFilter<T>(
        this IEnumerable<T> query, string jsonFilter)
    {
        var filterBuilder = new FilterBuilder();
        var filterExpression = filterBuilder.GetExpression<T>(jsonFilter);

        if (filterExpression is null)
        {
            throw new ArgumentException("Invalid filter");
        }

        var predicate = filterExpression.Compile();
        return query.Where(predicate);
    }

    public static IEnumerable<T> WithFilter<T>(
        this IEnumerable<T> query, string propertyName, Filter filter)
    {
        var filterBuilder = new FilterBuilder();
        var filterExpression = filterBuilder.GetExpression<T>(propertyName, filter);

        if (filterExpression is null)
        {
            throw new ArgumentException("Invalid filter");
        }

        var predicate = filterExpression.Compile();
        return query.Where(predicate);
    }

    public static IEnumerable<T> WithFilter<T>(
        this IEnumerable<T> query, Dictionary<string, Filter> filters)
    {
        var filterBuilder = new FilterBuilder();
        var filterExpression = filterBuilder.GetExpression<T>(filters);

        if (filterExpression is null)
        {
            throw new ArgumentException("Invalid filter");
        }

        var predicate = filterExpression.Compile();
        return query.Where(predicate);
    }
}