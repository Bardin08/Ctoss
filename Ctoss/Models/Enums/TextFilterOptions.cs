namespace Ctoss.Models.Enums;

/// <summary>
/// Specifies the available options for text filtering.
/// </summary>
public enum TextFilterOptions
{
    /// <summary>
    /// Provides an option to choose one of the other filter options.
    /// Option Key: empty
    /// Included by Default: No
    /// </summary>
    Empty = 0,

    /// <summary>
    /// Filters for text that contains the specified value.
    /// Option Key: contains
    /// Included by Default: Yes
    /// </summary>
    Contains = 1,

    /// <summary>
    /// Filters for text that does not contain the specified value.
    /// Option Key: notContains
    /// Included by Default: Yes
    /// </summary>
    NotContains = 2,

    /// <summary>
    /// Filters for text that is equal to the specified value.
    /// Option Key: equals
    /// Included by Default: Yes
    /// </summary>
    Equals = 3,

    /// <summary>
    /// Filters for text that is not equal to the specified value.
    /// Option Key: notEqual
    /// Included by Default: Yes
    /// </summary>
    NotEquals = 4,

    /// <summary>
    /// Filters for text that starts with the specified value.
    /// Option Key: startsWith
    /// Included by Default: Yes
    /// </summary>
    StartsWith = 5,

    /// <summary>
    /// Filters for text that ends with the specified value.
    /// Option Key: endsWith
    /// Included by Default: Yes
    /// </summary>
    EndsWith = 6,

    /// <summary>
    /// Filters for blank (null or empty) text.
    /// Option Key: blank
    /// Included by Default: Yes
    /// </summary>
    Blank = 7,

    /// <summary>
    /// Filters for text that is not blank (not null or empty).
    /// Option Key: notBlank
    /// Included by Default: Yes
    /// </summary>
    NotBlank = 8
}
