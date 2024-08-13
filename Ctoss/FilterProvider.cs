using System.Linq.Expressions;
using Ctoss.Builders.Filters;
using Ctoss.Extensions;
using Ctoss.Models;

namespace Ctoss;

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