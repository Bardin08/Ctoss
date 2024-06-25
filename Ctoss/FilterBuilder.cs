using System.Linq.Expressions;
using System.Text.Json;
using Ctoss.Extensions;
using Ctoss.Json;
using Ctoss.Models;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;

namespace Ctoss;

public class FilterBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters =
        {
            new FilterConditionConverter(),
            new JsonStringEnumConverter<Operator>(),
            new JsonStringEnumConverter<TextFilterOptions>(),
            new JsonStringEnumConverter<DateFilterOptions>(),
            new JsonStringEnumConverter<NumberFilterOptions>()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public Expression<Func<T, bool>>? GetExpression<T>(Dictionary<string, NumberFilter>? filters)
    {
        if (filters == null)
            return null;

        var expressions = new List<Expression<Func<T, bool>>>();

        expressions.AddRange(filters.Select(filter => GetExpressionInternal<T>(filter.Key, filter.Value)));
        return expressions.Aggregate((acc, expr) => acc.AndAlso(expr));
    }

    public Expression<Func<T, bool>>? GetExpression<T>(string jsonFilter)
        => GetExpression<T>(JsonSerializer.Deserialize<Dictionary<string, NumberFilter>>(jsonFilter, JsonOptions));

    public Expression<Func<T, bool>>? GetExpression<T>(string property, NumberFilter numberFilter)
        => GetExpression<T>(new Dictionary<string, NumberFilter> { { property, numberFilter } });

    private Expression<Func<T, bool>> GetExpressionInternal<T>(string property, NumberFilter? filter)
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

    private static Expression<Func<T, bool>> GetFilterExpr<T>(string property, FilterCondition? condition)
    {
        return condition switch
        {
            TextFilterCondition textFilter => GetTextFilterExpr<T>(property, textFilter),
            NumberFilterCondition numberFilter => GetNumberFilterExpr<T>(property, numberFilter),
            DateFilterCondition dateFilter => GetDateFilterExpr<T>(property, dateFilter),
            _ => _ => true
        };
    }

    private static Expression<Func<T, bool>> GetTextFilterExpr<T>(string property, TextFilterCondition condition)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);
        var valueExpression = Expression.Constant(condition.Filter);

        return condition.Type switch
        {
            TextFilterOptions.Contains => Expression.Lambda<Func<T, bool>>(
                Expression.Call(propertyExpression, nameof(string.Contains), null, valueExpression), parameter),
            TextFilterOptions.NotContains => Expression.Lambda<Func<T, bool>>(
                Expression.Not(
                    Expression.Call(propertyExpression, nameof(string.Contains), null, valueExpression)), parameter),
            TextFilterOptions.Equals => Expression.Lambda<Func<T, bool>>(
                Expression.Equal(propertyExpression, valueExpression), parameter),
            TextFilterOptions.NotEquals => Expression.Lambda<Func<T, bool>>(
                Expression.NotEqual(propertyExpression, valueExpression), parameter),
            TextFilterOptions.StartsWith => Expression.Lambda<Func<T, bool>>(
                Expression.Call(propertyExpression, nameof(string.StartsWith), null, valueExpression), parameter),
            TextFilterOptions.EndsWith => Expression.Lambda<Func<T, bool>>(
                Expression.Call(propertyExpression, nameof(string.EndsWith), null, valueExpression), parameter),
            TextFilterOptions.Blank => Expression.Lambda<Func<T, bool>>(
                Expression.Equal(propertyExpression, Expression.Constant(null, typeof(string))), parameter),
            TextFilterOptions.NotBlank => Expression.Lambda<Func<T, bool>>(
                Expression.NotEqual(propertyExpression, Expression.Constant(null, typeof(string))), parameter),
            _ => _ => true
        };
    }

    private static Expression<Func<T, bool>> GetNumberFilterExpr<T>(string property, NumberFilterCondition numberFilter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);

        var propertyType = typeof(T).GetProperty(property)?.PropertyType;
        if (propertyType == null)
            throw new ArgumentException($"Property '{property}' not found on type '{typeof(T).Name}'");

        var filterValue = Convert.ChangeType(numberFilter.Filter, propertyType);
        var valueExpression = Expression.Constant(filterValue, propertyType);

        switch (numberFilter.Type)
        {
            case NumberFilterOptions.Equals:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(propertyExpression, valueExpression), parameter);
            case NumberFilterOptions.NotEquals:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(propertyExpression, valueExpression), parameter);
            case NumberFilterOptions.GreaterThan:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.GreaterThan(propertyExpression, valueExpression), parameter);
            case NumberFilterOptions.GreaterThanOrEqual:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.GreaterThanOrEqual(propertyExpression, valueExpression), parameter);
            case NumberFilterOptions.LessThan:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.LessThan(propertyExpression, valueExpression), parameter);
            case NumberFilterOptions.LessThanOrEqual:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.LessThanOrEqual(propertyExpression, valueExpression), parameter);
            case NumberFilterOptions.InRange:
                var filterToValue = Convert.ChangeType(numberFilter.FilterTo, propertyType);
                var valueToExpression = Expression.Constant(filterToValue, propertyType);
                var greaterThan = Expression.GreaterThan(propertyExpression, valueExpression);
                var lessThan = Expression.LessThan(propertyExpression, valueToExpression);
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(greaterThan, lessThan), parameter);
            case NumberFilterOptions.Blank:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(propertyExpression, Expression.Constant(null, typeof(decimal?))), parameter);
            case NumberFilterOptions.Empty:
            case NumberFilterOptions.NotBlank:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(propertyExpression, Expression.Constant(null, typeof(decimal?))), parameter);
            default:
                throw new NotSupportedException($"Number filter type '{numberFilter.Type}' is not supported.");
        }
    }

    private static Expression<Func<T, bool>> GetDateFilterExpr<T>(string property, DateFilterCondition dateFilter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);

        ConstantExpression dateFromExpression = null!;

        if (dateFilter.Type is not DateFilterOptions.Blank &&
            dateFilter.Type is not DateFilterOptions.NotBlank &&
            dateFilter.Type is not DateFilterOptions.Empty)
        {
            dateFromExpression = Expression.Constant(DateTime.Parse(dateFilter.DateFrom!), typeof(DateTime));
        }

        switch (dateFilter.Type)
        {
            case DateFilterOptions.Equals:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(propertyExpression, dateFromExpression), parameter);
            case DateFilterOptions.NotEquals:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(propertyExpression, dateFromExpression), parameter);
            case DateFilterOptions.LessThen:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.LessThan(propertyExpression, dateFromExpression), parameter);
            case DateFilterOptions.GreaterThen:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.GreaterThan(propertyExpression, dateFromExpression), parameter);
            case DateFilterOptions.InRange:
                var dateToExpression = Expression.Constant(DateTime.Parse(dateFilter.DateTo!), typeof(DateTime));
                var greaterThan = Expression.GreaterThan(propertyExpression, dateFromExpression);
                var lessThan = Expression.LessThan(propertyExpression, dateToExpression);
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(greaterThan, lessThan), parameter);
            case DateFilterOptions.Empty:
            case DateFilterOptions.Blank:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Convert(propertyExpression, typeof(DateTime?)),
                        Expression.Constant(null, typeof(DateTime?))), parameter);
            case DateFilterOptions.NotBlank:
                return Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(
                        Expression.Convert(propertyExpression, typeof(DateTime?)),
                        Expression.Constant(null, typeof(DateTime?))), parameter);
            default:
                throw new NotSupportedException($"Date filter type '{dateFilter.Type}' is not supported.");
        }
    }
}
