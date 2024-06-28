using System.Linq.Expressions;
using Ctoss.Configuration;
using Ctoss.Expressions;

namespace Ctoss.Filters;

public interface IPropertyFilterBuilder<in TCondition>
{
    Expression<Func<T, bool>> GetExpression<T>(string property, TCondition condition);

    static UnaryExpression GetPropertyExpression<T>(
        string property,
        ParameterExpression parameter,
        Type propertyType)
    {
        var customMapping = CtossSettings.GetPropertyMapping<T>(property);
        Expression propertyRawExpression;

        if (customMapping == null)
        {
            propertyRawExpression = Expression.Property(parameter, property);
        }
        else
        {
            var visitor = new ReplaceParameterVisitor(customMapping.Parameters[0], parameter);
            propertyRawExpression = visitor.Visit(customMapping.Body);
        }

        var propertyExpression = Expression.Convert(propertyRawExpression, propertyType);
        return propertyExpression;
    }

    static Type GetPropertyType<T>(string property)
    {
        var customMappingType = CtossSettings.GetPropertyType<T>(property);
        if (customMappingType != null)
            return customMappingType;

        var propertyType = typeof(T).GetProperty(property)?.PropertyType;
        if (propertyType == null)
            throw new ArgumentException($"Property '{property}' not found on type '{typeof(T).Name}'");

        return propertyType;
    }
}
