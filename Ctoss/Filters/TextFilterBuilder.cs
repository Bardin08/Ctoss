using System.Linq.Expressions;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;

namespace Ctoss.Filters;

public class TextFilterBuilder : IPropertyFilterBuilder<TextFilterCondition>
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, TextFilterCondition condition)
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
}
