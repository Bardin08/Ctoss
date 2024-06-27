using System.Linq.Expressions;

namespace Ctoss.Configuration;

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
        Action<PropertySettings<TProp>>? propertyConfigurator = null)
    {
        _typeSettings.PropertyMappings.Properties[name] = (propertyExpression as Expression<Func<TEntity, object?>>)!;
        if (propertyConfigurator == null)
            return this;

        var propSettings = new PropertySettings<TProp>();
        propertyConfigurator(propSettings);
        _typeSettings.PropertySettings[name] = propSettings;

        return this;
    }

    public TypeSettingsBuilder<TEntity> Property<TProp>(
        Expression<Func<TEntity, TProp?>> propertyExpression,
        Action<PropertySettings<TProp>>? propertyConfigurator = null)
    {
        var propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;
        _typeSettings.PropertyMappings.Properties[propertyName] =
            (propertyExpression as Expression<Func<TEntity, object?>>)!;

        if (propertyConfigurator == null)
            return this;

        var propSettings = new PropertySettings<TProp>();
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
