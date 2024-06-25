namespace Ctoss.Models.Enums;

/// <summary>
/// Specifies the available options for number filtering.
/// </summary>
public enum NumberFilterOptions
{
    /// <summary>
    /// Provides an option to choose one of the other filter options.
    /// Option Key: empty
    /// Included by Default: No
    /// </summary>
    Empty = 0,

    /// <summary>
    /// Filters for numbers that are equal to the specified value.
    /// Option Key: equals
    /// Included by Default: Yes
    /// </summary>
    Equals = 1,

    /// <summary>
    /// Filters for numbers that are not equal to the specified value.
    /// Option Key: notEqual
    /// Included by Default: Yes
    /// </summary>
    NotEquals = 2,

    /// <summary>
    /// Filters for numbers that are greater than the specified value.
    /// Option Key: greaterThan
    /// Included by Default: Yes
    /// </summary>
    GreaterThan = 3,

    /// <summary>
    /// Filters for numbers that are greater than or equal to the specified value.
    /// Option Key: greaterThanOrEqual
    /// Included by Default: Yes
    /// </summary>
    GreaterThanOrEqual = 4,

    /// <summary>
    /// Filters for numbers that are less than the specified value.
    /// Option Key: lessThan
    /// Included by Default: Yes
    /// </summary>
    LessThan = 5,

    /// <summary>
    /// Filters for numbers that are less than or equal to the specified value.
    /// Option Key: lessThanOrEqual
    /// Included by Default: Yes
    /// </summary>
    LessThanOrEqual = 6,

    /// <summary>
    /// Filters for numbers that are within a specified range.
    /// Option Key: inRange
    /// Included by Default: Yes
    /// </summary>
    InRange = 7,

    /// <summary>
    /// Filters for blank (null or empty) numbers.
    /// Option Key: blank
    /// Included by Default: Yes
    /// </summary>
    Blank = 8,

    /// <summary>
    /// Filters for numbers that are not blank (not null or empty).
    /// Option Key: notBlank
    /// Included by Default: Yes
    /// </summary>
    NotBlank = 9
}
