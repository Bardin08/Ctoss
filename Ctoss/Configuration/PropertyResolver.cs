using System.Linq.Expressions;

namespace Ctoss.Configuration;

public class PropertyResolver<TEntity>
{
    public Dictionary<string, Expression<Func<TEntity, object?>>> Properties { get; set; } = new();
}