using System.Collections.Concurrent;
using System.Linq.Expressions;
using Ctoss.Core.Builders.Sorting;
using Ctoss.Models;

namespace Ctoss.Core;

public class SortingProvider : ISortingProvider
{
    private readonly ISortingBuilder _sortingBuilder;
    private readonly ISortingExpressionCache _expressionExpressionCache;

    public SortingProvider(ISortingBuilder sortingBuilder, ISortingExpressionCache expressionExpressionCache)
    {
        _sortingBuilder = sortingBuilder;
        _expressionExpressionCache = expressionExpressionCache;
    }

    public Expression<Func<TModel, object>>? GetSortingExpression<TModel>(Sorting sorting)
    {
        var cachedExpression = (Expression<Func<TModel, object>>?)_expressionExpressionCache.Get(sorting);

        var cacheExists = cachedExpression is not null;
        if (cacheExists)
        {
            return cachedExpression;
        }

        var generatedExpression = _sortingBuilder.BuildSortingExpressionV2<TModel>(sorting);
        _expressionExpressionCache.Set(sorting, generatedExpression!);

        return generatedExpression;

    }
}

public interface ISortingExpressionCache : ICache<Sorting, object>
{
}

public class SortingExpressionCache : ISortingExpressionCache
{
    private readonly ConcurrentDictionary<Sorting, object> _cache = new();

    public object? Get(Sorting key)
    {
        _ = _cache.TryGetValue(key, out var entry);
        return entry;
    }

    public void Set(Sorting key, object entry)
    {
        _cache.AddOrUpdate(key, _ => entry, (_, _) => entry);
    }
}