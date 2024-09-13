using System.Linq.Expressions;

namespace Ctoss.Builders.Sorting;

public class SortingBuilder
{
    public Expression<Func<T, object>>? BuildSortingExpressionV2<T>(Models.Sorting? sorting, bool conditionalAccess)
        => sorting == null ? null : GetPropertyExpressionInternal<T>(sorting.Property, conditionalAccess);

    private Expression<Func<T, object>> GetPropertyExpressionInternal<T>(string property, bool conditionalAccess)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = IPropertyBuilder.GetCompletePropertyExpression<T>(property, parameter, conditionalAccess);

        if (propertyExpression == null)
            throw new ArgumentException($"Property '{property}' not found on type '{typeof(T).Name}' or configuration");

        var converted = Expression.Convert(propertyExpression, typeof(object));
        return Expression.Lambda<Func<T, object>>(converted, parameter);
    }
}