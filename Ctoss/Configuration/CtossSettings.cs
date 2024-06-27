namespace Ctoss.Configuration;

public class CtossSettings
{
    public Dictionary<Type, ITypeSettings> TypeSettings { get; set; } = new();
}
