namespace Ctoss.Configuration;

/// <summary>
/// Represents the settings for a specific property of type <typeparamref name="TProp"/>.
/// </summary>
/// <typeparam name="TProp">The type of the property.</typeparam>
public class PropertySettings<TProp> : IPropertySettings
{
    /// <summary>
    /// Gets or sets the custom converter function that converts a string to the property type <typeparamref name="TProp"/>.
    /// </summary>
    /// <value>
    /// A function that converts a string to the property type <typeparamref name="TProp"/>. The function should accept a string parameter and return a value of type <typeparamref name="TProp"/>.
    /// </value>
    public Func<string, TProp>? CustomConverter { get; set; }

    /// <inheritdoc />
    public bool Required { get; set; }

    /// <inheritdoc />
    public bool IgnoreCase { get; set; }
}
