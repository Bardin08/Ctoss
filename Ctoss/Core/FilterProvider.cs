using System.Linq.Expressions;
using Ctoss.Core.Builders.Filters.Abstractions;
using Ctoss.Extensions;
using Ctoss.Models;

namespace Ctoss.Core;

public class FilterProvider : IFilterProvider
{
    private readonly IFilterBuilder _filterBuilder;
    private readonly IFilterExpressionCache _expressionCache;

    public FilterProvider(IFilterBuilder filterBuilder, IFilterExpressionCache expressionCache)
    {
        _filterBuilder = filterBuilder;
        _expressionCache = expressionCache;
    }

    public Expression<Func<TModel, bool>>? GetFilterExpression<TModel>(
        IEnumerable<FilterDescriptor>? filterSet)
    {
        var aggregatedExpression = filterSet?.Select(GetExpression<TModel>)
            .OfType<Expression<Func<TModel, bool>>>()
            .Aggregate((acc, expr) => acc.AndAlso(expr));

        return aggregatedExpression;
    }

    private Expression<Func<TModel, bool>>? GetExpression<TModel>(FilterDescriptor filter)
    {
        var cachedExpression = (Expression<Func<TModel, bool>>?)_expressionCache.Get(filter);

        var cacheExists = cachedExpression is not null;
        if (cacheExists)
        {
            return cachedExpression;
        }

        var generatedExpression = _filterBuilder.GetExpression<TModel>(filter);
        _expressionCache.Set(filter, generatedExpression!);

        return generatedExpression;
    }
}