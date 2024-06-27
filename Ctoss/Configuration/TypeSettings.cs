namespace Ctoss.Configuration;

public class TypeSettings<TEntity> : ITypeSettings
{
    public PropertyResolver<TEntity> PropertyMappings { get; set; } = new();

    public Dictionary<string, IPropertySettings> PropertySettings { get; set; } = new();

    /// <summary>
    /// This has performance implications.
    /// If you are using this in a high-performance scenario, consider keeping this to false.
    /// </summary>
    public bool IgnoreCase { get; set; } = false;
}
