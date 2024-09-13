using System.Linq.Expressions;
using Ctoss.Configuration;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;

namespace Ctoss.Builders.Filters;

public class TextFilterBuilder : IPropertyFilterBuilder<TextCondition>
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, TextCondition condition, bool conditionalAccess)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = IPropertyFilterBuilder<T>
            .GetPropertyExpression<T>(property, parameter, typeof(string), conditionalAccess);
        propertyExpression = Expression.Coalesce(propertyExpression, Expression.Constant(string.Empty));
        var valueExpression = Expression.Constant(condition.Filter);

        
        ApplyPropertySettings<T>(property, ref propertyExpression, ref valueExpression);
        return condition.Type switch
        {
            TextFilterOptions.Contains
                => Expression.Lambda<Func<T, bool>>(
                    Expression.Call(propertyExpression, nameof(string.Contains), null, valueExpression),
                    parameter
                ),

            TextFilterOptions.NotContains
                => Expression.Lambda<Func<T, bool>>(
                    Expression.Not(
                        Expression.Call(propertyExpression, nameof(string.Contains), null, valueExpression)
                    ),
                    parameter
                ),

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

    private static void ApplyPropertySettings<T>(
        string property,
        ref Expression propertyExpression,
        ref ConstantExpression valueExpression)
    {
        var propertySettings = CtossSettings.GetPropertySettings<T>(property);
        if (propertySettings is not { IgnoreCase: true })
        {
            return;
        }

        var memberExpression = propertyExpression is UnaryExpression unary ? unary.Operand : propertyExpression;

        var propertyToUpper = Expression.Call(memberExpression, nameof(string.ToUpper), Type.EmptyTypes);
        propertyExpression = Expression.Convert(propertyToUpper, typeof(string));

        if (valueExpression.Value is not string value)
        {
            return;
        }

        valueExpression = Expression.Constant(value.ToUpper());
    }
}
