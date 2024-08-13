using System.Linq.Expressions;
using System.Reflection;
using Ctoss.Builders.Filters.Abstractions;
using Ctoss.Extensions;
using Ctoss.Models;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;

namespace Ctoss.Builders.Filters;

public class FilterBuilder : IFilterBuilder
{
    private readonly ITextFilterBuilder _textFilterBuilder;
    private readonly IDateFilterBuilder _dateFilterBuilder;
    private readonly INumberFilterBuilder _numberFilterBuilder;
    private readonly ISetFilterBuilder _setFilterBuilder;

    public FilterBuilder(
        ITextFilterBuilder textFilterBuilder,
        IDateFilterBuilder dateFilterBuilder,
        INumberFilterBuilder numberFilterBuilder,
        ISetFilterBuilder setFilterBuilder)
    {
        _textFilterBuilder = textFilterBuilder;
        _dateFilterBuilder = dateFilterBuilder;
        _numberFilterBuilder = numberFilterBuilder;
        _setFilterBuilder = setFilterBuilder;
    }

    public Expression<Func<T, bool>>? GetExpression<T>(FilterDescriptor? descriptor)
    {
        return descriptor != null
            ? GetExpressionInternal<T>(descriptor.PropertyName, descriptor.Filter)
            : null;
    }

    public Expression<Func<T, bool>>? GetExpression<T>(Dictionary<string, FilterModel>? filterSet)
    {
        if (filterSet == null)
            return null;

        var expressions = new List<Expression<Func<T, bool>>>();

        expressions.AddRange(filterSet
            .Select(filter => GetExpressionInternal<T>(filter.Key, filter.Value)));
        return expressions.Aggregate((acc, expr) => acc.AndAlso(expr));
    }

    public Expression<Func<T, bool>>? GetExpression<T>(string property, FilterModel filter)
        => GetExpression<T>(new Dictionary<string, FilterModel> { { property, filter } });

    private Expression<Func<T, bool>> GetExpressionInternal<T>(string property, FilterModel? filter)
    {
        if (filter == null)
            return _ => true;

        if (filter.Operator != null && filter.Operator != Operator.NoOp)
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

        return GetFilterExpr<T>(property, MapPlainFilterToConditions(filter));
    }

    private static FilterConditionBase MapPlainFilterToConditions(FilterModel filter)
    {
        if (filter.Conditions is not null && filter.Conditions?.Count != 0)
            throw new ArgumentException("The given filter is not a plain filter");

        return filter.FilterType switch
        {
            "text" => new TextCondition
            {
                Type = Enum.Parse<TextFilterOptions>(filter.Type, ignoreCase: true),
                Filter = filter.Filter!,
                FilterType = filter.FilterType
            },
            "date" => new DateCondition
            {
                Type = Enum.Parse<DateFilterOptions>(filter.Type, ignoreCase: true),
                DateFrom = filter.DateFrom,
                DateTo = filter.DateTo,
                FilterType = filter.FilterType
            },
            "number" => new NumberCondition
            {
                Type = Enum.Parse<NumberFilterOptions>(filter.Type, ignoreCase: true),
                Filter = filter.Filter!,
                FilterTo = filter.FilterTo!,
                FilterType = filter.FilterType
            },
            "set" => new SetCondition
            {
                FilterType = filter.FilterType,
                Values = filter.Values
            },
            _ => throw new ArgumentException("Unknown filter type")
        };
    }

    private Expression<Func<T, bool>> GetFilterExpr<T>(string property, FilterConditionBase? condition)
    {
        // NOTE: first of all, we're trying to get a real property name from the given one.
        // If we find it, we can use it to work with an expression. Else the given property name will be used.
        var normalizedProperty = typeof(T)
            .GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        var propertyName = normalizedProperty?.Name ?? property;
        return condition switch
        {
            TextCondition textCondition
                => _textFilterBuilder.GetExpression<T>(propertyName, textCondition),
            DateCondition dateCondition
                => _dateFilterBuilder.GetExpression<T>(propertyName, dateCondition),
            NumberCondition numberCondition
                => _numberFilterBuilder.GetExpression<T>(propertyName, numberCondition),
            SetCondition setCondition
                => _setFilterBuilder.GetExpression<T>(propertyName, setCondition),
            _ => _ => true
        };
    }
}