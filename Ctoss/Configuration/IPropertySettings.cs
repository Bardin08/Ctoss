namespace Ctoss.Configuration;

public interface IPropertySettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the property is required.
    /// </summary>
    /// <value>
    /// <c>true</c> if the property is required; otherwise, <c>false</c>.
    /// </value>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the case should be ignored when resolving the property.
    /// </summary>
    /// <value>
    /// <c>true</c> if the case should be ignored; otherwise, <c>false</c>.
    /// </value>
    public bool IgnoreCase { get; set; }
}
