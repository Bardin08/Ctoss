using System.Linq.Expressions;
using System.Text.Json;
using Ctoss.Extensions;
using Ctoss.Json;
using Ctoss.Models;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;

namespace Ctoss.Filters;

public class FilterBuilder
{
    private readonly IPropertyFilterBuilder<TextFilterCondition> _textFilterBuilder = new TextFilterBuilder();
    private readonly IPropertyFilterBuilder<DateFilterCondition> _dateFilterBuilder = new DateFilterBuilder();
    private readonly IPropertyFilterBuilder<NumberFilterCondition> _numberFilterBuilder = new NumberFilterBuilder();

    public Expression<Func<T, bool>>? GetExpression<T>(Dictionary<string, Filter>? filters)
    {
        if (filters == null)
            return null;

        var expressions = new List<Expression<Func<T, bool>>>();

        expressions.AddRange(filters.Select(filter => GetExpressionInternal<T>(filter.Key, filter.Value)));
        return expressions.Aggregate((acc, expr) => acc.AndAlso(expr));
    }

    public Expression<Func<T, bool>>? GetExpression<T>(string jsonFilter)
        => GetExpression<T>(
            JsonSerializer.Deserialize<Dictionary<string, Filter>>(jsonFilter, CtossJsonDefaults.DefaultJsonOptions));

    public Expression<Func<T, bool>>? GetExpression<T>(string property, Filter filter)
        => GetExpression<T>(new Dictionary<string, Filter> { { property, filter } });

    private Expression<Func<T, bool>> GetExpressionInternal<T>(string property, Filter? filter)
    {
        if (filter == null)
            return _ => true;

        if (filter.Operator != Operator.NoOp)
        {
            return filter.Conditions?
                .Select(c => GetFilterExpr<T>(property, c))
                .Aggregate((acc, expr) => filter.Operator switch
                {
                    Operator.And => acc.AndAlso(expr),
                    Operator.Or => acc.OrElse(expr),
                    _ => throw new ArgumentOutOfRangeException()
                })!;
        }

        return GetFilterExpr<T>(property, filter.Condition1);
    }

    private Expression<Func<T, bool>> GetFilterExpr<T>(string property, FilterCondition? condition)
    {
        return condition switch
        {
            TextFilterCondition textCondition => _textFilterBuilder.GetExpression<T>(property, textCondition),
            DateFilterCondition dateCondition => _dateFilterBuilder.GetExpression<T>(property, dateCondition),
            NumberFilterCondition numberCondition => _numberFilterBuilder.GetExpression<T>(property, numberCondition),
            _ => _ => true
        };
    }
}
