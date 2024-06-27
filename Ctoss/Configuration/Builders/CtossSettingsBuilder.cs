namespace Ctoss.Configuration;

public class CtossSettingsBuilder
{
    private readonly CtossSettings _settings = new();

    private CtossSettingsBuilder()
    {
    }

    public static CtossSettingsBuilder Create() => new();

    public TypeSettingsBuilder<TEntity> Entity<TEntity>()
    {
        return new TypeSettingsBuilder<TEntity>(this);
    }

    internal CtossSettingsBuilder AddSettings<TEntity>(TypeSettings<TEntity> typeSettings)
    {
        _settings.TypeSettings[typeof(TEntity)] = typeSettings;
        return this;
    }

    public CtossSettings Build() => _settings;
}