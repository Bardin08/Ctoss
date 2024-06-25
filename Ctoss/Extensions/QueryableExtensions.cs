using Ctoss.Models;

namespace Ctoss.Extensions;

public static class QueryableExtensions
{
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

        return query.OrderBy(sortExpression);
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

        return query.OrderBy(sortExpression);
    }
}
