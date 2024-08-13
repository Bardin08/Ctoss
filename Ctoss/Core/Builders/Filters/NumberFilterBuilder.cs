using System.Linq.Expressions;
using Ctoss.Core.Builders.Filters.Abstractions;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;

namespace Ctoss.Core.Builders.Filters;

public class NumberFilterBuilder : INumberFilterBuilder
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, NumberCondition condition)
    {
        return condition.Type switch
        {
            NumberFilterOptions.Blank or NumberFilterOptions.NotBlank or NumberFilterOptions.Empty
                => GetBlankExpression<T>(property, condition),
            NumberFilterOptions.InRange
                => GetRangeExpression<T>(property, condition),
            _ => GetComparisonExpression<T>(property, condition)
        };
    }

    private Expression<Func<T, bool>> GetBlankExpression<T>(string property, NumberCondition condition)
    {
        var propertyType = IPropertyBuilder.GetPropertyType<T>(property);

        var nullablePropertyType = propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null
            ? typeof(Nullable<>).MakeGenericType(propertyType)
            : propertyType;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = IPropertyBuilder.GetPropertyExpression<T>(property, parameter, propertyType);

        return condition.Type switch
        {
            NumberFilterOptions.Blank
                => Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Convert(propertyExpression, nullablePropertyType),
                        Expression.Constant(null, nullablePropertyType)
                    ), parameter),

            NumberFilterOptions.Empty or NumberFilterOptions.NotBlank
                => Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(
                        Expression.Convert(propertyExpression, nullablePropertyType),
                        Expression.Constant(null, nullablePropertyType)
                    ), parameter),

            _ => throw new NotSupportedException($"Number filter type '{condition.Type}' is not supported.")
        };
    }

    private Expression<Func<T, bool>> GetRangeExpression<T>(string property, NumberCondition condition)
    {
        if (string.IsNullOrEmpty(condition.Filter))
            throw new ArgumentException("Filter value is required.");

        if (string.IsNullOrEmpty(condition.FilterTo))
            throw new ArgumentException("FilterTo value is required.");

        var propertyType = IPropertyBuilder.GetPropertyType<T>(property);

        var from = ParseNumericValue(condition.Filter, propertyType);
        var fromExpression = Expression.Constant(from, propertyType);

        var to = ParseNumericValue(condition.FilterTo, propertyType);
        var toExpression = Expression.Constant(to, propertyType);

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = IPropertyBuilder.GetPropertyExpression<T>(property, parameter, propertyType);

        var greaterThan = Expression.GreaterThan(propertyExpression, fromExpression);
        var lessThan = Expression.LessThan(propertyExpression, toExpression);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(greaterThan, lessThan),
            parameter
        );
    }

    private Expression<Func<T, bool>> GetComparisonExpression<T>(string property, NumberCondition condition)
    {
        if (string.IsNullOrEmpty(condition.Filter))
            throw new ArgumentException("Filter value is required.");

        var propertyType = IPropertyBuilder.GetPropertyType<T>(property);

        var from = ParseNumericValue(condition.Filter, propertyType);
        var fromExpression = Expression.Constant(from, propertyType);

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = IPropertyBuilder.GetPropertyExpression<T>(property, parameter, propertyType);

        return condition.Type switch
        {
            NumberFilterOptions.Equals
                => Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(propertyExpression, fromExpression),
                    parameter
                ),
            NumberFilterOptions.NotEquals
                => Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(propertyExpression, fromExpression),
                    parameter
                ),
            NumberFilterOptions.GreaterThan
                => Expression.Lambda<Func<T, bool>>(
                    Expression.GreaterThan(propertyExpression, fromExpression),
                    parameter
                ),
            NumberFilterOptions.GreaterThanOrEqual
                => Expression.Lambda<Func<T, bool>>(
                    Expression.GreaterThanOrEqual(propertyExpression, fromExpression),
                    parameter
                ),
            NumberFilterOptions.LessThan
                => Expression.Lambda<Func<T, bool>>(
                    Expression.LessThan(propertyExpression, fromExpression),
                    parameter
                ),
            NumberFilterOptions.LessThanOrEqual
                => Expression.Lambda<Func<T, bool>>(
                    Expression.LessThanOrEqual(propertyExpression, fromExpression),
                    parameter
                ),
            _ => throw new NotSupportedException($"Number filter type '{condition.Type}' is not supported.")
        };
    }

    private static object ParseNumericValue(string filterValue, Type propertyType)
    {
        if (Nullable.GetUnderlyingType(propertyType) is null)
            return Convert.ChangeType(filterValue, propertyType);

        if (propertyType == typeof(int?)) return int.Parse(filterValue);
        if (propertyType == typeof(long?)) return long.Parse(filterValue);
        if (propertyType == typeof(decimal?)) return decimal.Parse(filterValue);
        if (propertyType == typeof(double?)) return double.Parse(filterValue);
        if (propertyType == typeof(float?)) return float.Parse(filterValue);
        if (propertyType == typeof(short?)) return short.Parse(filterValue);
        if (propertyType == typeof(byte?)) return byte.Parse(filterValue);
        if (propertyType == typeof(sbyte?)) return sbyte.Parse(filterValue);
        if (propertyType == typeof(ushort?)) return ushort.Parse(filterValue);
        if (propertyType == typeof(uint?)) return uint.Parse(filterValue);
        if (propertyType == typeof(ulong?)) return ulong.Parse(filterValue);

        return Convert.ChangeType(filterValue, propertyType);
    }
}
