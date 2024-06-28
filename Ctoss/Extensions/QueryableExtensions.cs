using System.Text.Json;
using Ctoss.Builders.Filters;
using Ctoss.Builders.Sorting;
using Ctoss.Models;
using Ctoss.Models.Enums;

namespace Ctoss.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, int page, int pageSize)
        => query.Skip((page - 1) * pageSize).Take(pageSize);

    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, Pagination pagination)
        => WithPagination(query, pagination.Page, pagination.PageSize);

    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, string jsonPagination)
        => WithPagination(query, JsonSerializer.Deserialize<Pagination>(jsonPagination)
                                 ?? throw new ArgumentException("Invalid pagination"));

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
            var sortingExpression = sortingBuilder.BuildSortingExpression<T>(sorting);

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

    public static IQueryable<T> WithFilter<T>(
        this IQueryable<T> query, string jsonFilter)
    {
        var filterBuilder = new FilterBuilder();
        var filterExpression = filterBuilder.GetExpression<T>(jsonFilter);

        if (filterExpression is null)
        {
            throw new ArgumentException("Invalid filter");
        }

        return query.Where(filterExpression);
    }

    public static IQueryable<T> WithFilter<T>(
        this IQueryable<T> query, string propertyName, Filter filter)
    {
        var sortBuilder = new FilterBuilder();
        var sortExpression = sortBuilder.GetExpression<T>(propertyName, filter);

        if (sortExpression is null)
        {
            throw new ArgumentException("Invalid filter");
        }

        return query.Where(sortExpression);
    }

    public static IQueryable<T> WithFilter<T>(
        this IQueryable<T> query, Dictionary<string, Filter> filters)
    {
        var sortBuilder = new FilterBuilder();
        var sortExpression = sortBuilder.GetExpression<T>(filters);

        if (sortExpression is null)
        {
            throw new ArgumentException("Invalid filter");
        }

        return query.Where(sortExpression);
    }
}
