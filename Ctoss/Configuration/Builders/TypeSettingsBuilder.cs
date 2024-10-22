using System.Linq.Expressions;

namespace Ctoss.Configuration.Builders;

public class TypeSettingsBuilder<TEntity>
{
    private readonly CtossSettingsBuilder _parentBuilder;
    private readonly TypeSettings<TEntity> _typeSettings = new();

    public TypeSettingsBuilder(CtossSettingsBuilder parentBuilder)
    {
        _parentBuilder = parentBuilder;
    }

    public TypeSettingsBuilder<TEntity> Property<TProp>(
        string name,
        Expression<Func<TEntity, TProp?>> propertyExpression,
        Action<PropertySettings<TEntity, TProp>>? propertyConfigurator = null)
    {
        var convertedExpression = Expression.Lambda<Func<TEntity, object?>>(
            Expression.Convert(propertyExpression.Body, typeof(object)),
            propertyExpression.Parameters);

        _typeSettings.PropertyMappings.Properties[name] = convertedExpression;
        if (propertyConfigurator == null)
            return this;

        var propSettings = new PropertySettings<TEntity, TProp>();
        propertyConfigurator(propSettings);
        _typeSettings.PropertySettings[name] = propSettings;

        return this;
    }

    public TypeSettingsBuilder<TEntity> Property<TProp>(
        Expression<Func<TEntity, TProp>> propertyExpression,
        Action<PropertySettings<TEntity, TProp>>? propertyConfigurator = null)
    {
        var propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;
        var convertedExpression = Expression.Lambda<Func<TEntity, object?>>(
            Expression.Convert(propertyExpression.Body, typeof(object)),
            propertyExpression.Parameters);
        
        _typeSettings.PropertyMappings.Properties[propertyName] = convertedExpression;

        if (propertyConfigurator == null)
            return this;

        var propSettings = new PropertySettings<TEntity, TProp>();
        propertyConfigurator(propSettings);
        _typeSettings.PropertySettings[propertyName] = propSettings;

        return this;
    }

    public TypeSettingsBuilder<TEntity> IgnoreCaseForEntity(bool ignoreCase)
    {
        _typeSettings.IgnoreCase = ignoreCase;
        return this;
    }

    public CtossSettingsBuilder Apply()
    {
        _parentBuilder.AddSettings(_typeSettings);
        return _parentBuilder;
    }
}
