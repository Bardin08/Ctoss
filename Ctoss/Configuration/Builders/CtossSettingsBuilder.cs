namespace Ctoss.Configuration.Builders;

public class CtossSettingsBuilder
{
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
        CtossSettings.TypeSettings[typeof(TEntity)] = typeSettings;
        return this;
    }
}
