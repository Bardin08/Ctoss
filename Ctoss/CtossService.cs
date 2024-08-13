using System.Collections.Concurrent;
using System.Linq.Expressions;
using Ctoss.Builders.Filters;
using Ctoss.Builders.Sorting;
using Ctoss.Extensions;
using Ctoss.Models;
using Ctoss.Models.AgGrid;

namespace Ctoss;

public interface ICtossService
{
    Task<AgGridQueryResult<T>> ApplyAsync<T>(AgGridQuery query, IEnumerable<T> enumerable);
}

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

            if (predicate is null)
                throw new ArgumentException("Invalid filter");

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

public interface IFilterProvider
{
    Task<Expression<Func<TModel, bool>>?> GetFilterExpressionAsync<TModel>(
        IEnumerable<FilterDescriptor>? filterModel);
}

public class FilterProvider : IFilterProvider
{
    private readonly IFilterBuilder _filterBuilder;
    private readonly IFilterExpressionCache _expressionCache;

    public FilterProvider(IFilterBuilder filterBuilder, IFilterExpressionCache expressionCache)
    {
        _filterBuilder = filterBuilder;
        _expressionCache = expressionCache;
    }

    public async Task<Expression<Func<TModel, bool>>?> GetFilterExpressionAsync<TModel>(
        IEnumerable<FilterDescriptor>? filterSet)
    {
        if (filterSet is null)
        {
            return null;
        }

        var filterExpressionsTasks = filterSet.Select(GetExpressionAsync<TModel>);
        var expressions = await Task.WhenAll(filterExpressionsTasks);

        var aggregatedExpression = expressions
            .OfType<Expression<Func<TModel, bool>>>()
            .Aggregate((acc, expr) => acc.AndAlso(expr));

        return aggregatedExpression;
    }

    private async Task<Expression<Func<TModel, bool>>?> GetExpressionAsync<TModel>(FilterDescriptor filter)
    {
        var cachedExpression = (Expression<Func<TModel, bool>>?)await _expressionCache.Get(filter)!;

        var cacheExists = cachedExpression is not null;
        if (cacheExists)
        {
            return cachedExpression;
        }

        var generatedExpression = _filterBuilder.GetExpression<TModel>(filter);
        await _expressionCache.Set(filter, generatedExpression!);

        return generatedExpression;
    }
}

public interface ICache<in TKey, TEntry>
{
    Task<object>? Get(TKey key);
    Task Set(TKey key, TEntry entry);
}

public interface IFilterExpressionCache : ICache<FilterDescriptor, object>
{
}

public class FilterExpressionCache : IFilterExpressionCache
{
    private readonly ConcurrentDictionary<FilterDescriptor, object> _cache = new();

    public Task<object>? Get(FilterDescriptor key)
    {
        _ = _cache.TryGetValue(key, out var entry);
        return Task.FromResult(entry)!;
    }

    public Task Set(FilterDescriptor key, object entry)
    {
        _cache.AddOrUpdate(key, _ => entry, (_, _) => entry);
        return Task.CompletedTask;
    }
}