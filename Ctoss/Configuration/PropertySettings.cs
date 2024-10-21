using System.Linq.Expressions;

namespace Ctoss.Configuration;

/// <summary>
/// Represents the settings for a specific property.
/// </summary>
public abstract class PropertySettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the case should be ignored when resolving the property.
    /// </summary>
    /// <value>
    /// <c>true</c> if the case should be ignored; otherwise, <c>false</c>.
    /// </value>
    public bool IgnoreCase { get; set; }

    public abstract Expression? GetSortValueExpression();
}

public abstract class PropertySettings<TEntity> : PropertySettings
{
    
}

public class PropertySettings<TEntity, TProperty> : PropertySettings<TEntity>
{
    /// <summary>
    /// Lambda expression that can convert property to value for ordering
    /// </summary>
    public Expression<Func<TProperty, int>>? SortValueExpression { get; set; }

    public override Expression? GetSortValueExpression() => SortValueExpression;
}
