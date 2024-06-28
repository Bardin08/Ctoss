using System.Linq.Expressions;

namespace Ctoss.Configuration;

public static class CtossSettings
{
    public static Dictionary<Type, ITypeSettings> TypeSettings { get; set; } = new();

    internal static Expression<Func<TEntity, object?>>? GetPropertyMapping<TEntity>(string propertyName)
    {
        var hasTypeConfiguration = TypeSettings.ContainsKey(typeof(TEntity));
        if (!hasTypeConfiguration)
            return null;

        var typeSettings = TypeSettings[typeof(TEntity)] as TypeSettings<TEntity>;
        if (typeSettings == null)
            return null;

        // We can't do case-insensitive search here because mapping is stored in the dictionary
        var hasPropertyConfiguration = typeSettings.PropertyMappings.Properties.ContainsKey(propertyName);
        return hasPropertyConfiguration
            ? typeSettings.PropertyMappings.Properties[propertyName]
            : null;
    }

    internal static Type? GetPropertyType<TEntity>(string propertyName)
    {
        var propertyMapping = GetPropertyMapping<TEntity>(propertyName);
        if (propertyMapping == null)
            return null;

        var unaryExpression = (UnaryExpression)propertyMapping.Body;
        var binaryExpression = (BinaryExpression)unaryExpression.Operand;

        var resultType = binaryExpression.Type;
        return resultType;
    }
}
