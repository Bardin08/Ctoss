using System.Linq.Expressions;

namespace Ctoss.Builders.Sorting;

public class SortingBuilder
{
    public Expression<Func<T, object>> BuildSortingExpression<T>(Models.Sorting sorting)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyType = IPropertyBuilder.GetPropertyType<T>(sorting.Property);
        var propertyExpression = IPropertyBuilder.GetPropertyExpression<T>(sorting.Property, parameter, propertyType);

        if (propertyExpression == null)
            throw new ArgumentException($"Property '{sorting.Property}' not found on type '{typeof(T).Name}'");

        var converted = Expression.Convert(propertyExpression, typeof(object));
        return Expression.Lambda<Func<T, object>>(converted, parameter);
    }
}
