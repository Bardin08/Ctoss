using System.Linq.Expressions;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;

namespace Ctoss.Filters;

public class DateFilterBuilder : IPropertyFilterBuilder<DateFilterCondition>
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, DateFilterCondition condition)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property);

        ConstantExpression dateFromExpression = null!;

        if (condition.Type is not DateFilterOptions.Blank &&
            condition.Type is not DateFilterOptions.NotBlank &&
            condition.Type is not DateFilterOptions.Empty)
        {
            dateFromExpression = Expression.Constant(DateTime.Parse(condition.DateFrom!), typeof(DateTime));
        }

        switch (condition.Type)
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
                var dateToExpression = Expression.Constant(DateTime.Parse(condition.DateTo!), typeof(DateTime));
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
                throw new NotSupportedException($"Date filter type '{condition.Type}' is not supported.");
        }
    }
}
