using System.Linq.Expressions;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;

namespace Ctoss.Filters;

public class NumberFilterBuilder : IPropertyFilterBuilder<NumberFilterCondition>
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, NumberFilterCondition condition)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);

        var propertyType = typeof(T).GetProperty(property)?.PropertyType;
        if (propertyType == null)
            throw new ArgumentException($"Property '{property}' not found on type '{typeof(T).Name}'");

        var filterValue = Convert.ChangeType(condition.Filter, propertyType);
        var valueExpression = Expression.Constant(filterValue, propertyType);

        switch (condition.Type)
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
                var filterToValue = Convert.ChangeType(condition.FilterTo, propertyType);
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
                throw new NotSupportedException($"Number filter type '{condition.Type}' is not supported.");
        }
    }
}
