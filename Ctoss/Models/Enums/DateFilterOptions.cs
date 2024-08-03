namespace Ctoss.Models.Enums;

/// <summary>
/// Specifies the available options for date filtering.
/// </summary>
public enum DateFilterOptions
{
    /// <summary>
    /// Provides an option to choose one of the other filter options.
    /// Option Key: empty
    /// Included by Default: No
    /// </summary>
    Empty = 0,

    /// <summary>
    /// Filters for dates that are equal to the specified date.
    /// Option Key: equals
    /// Included by Default: Yes
    /// </summary>
    Equals = 1,

    /// <summary>
    /// Filters for dates that are not equal to the specified date.
    /// Option Key: notEqual
    /// Included by Default: Yes
    /// </summary>
    NotEquals = 2,

    /// <summary>
    /// Filters for dates that are less than the specified date.
    /// Option Key: lessThan
    /// Included by Default: Yes
    /// </summary>
    LessThan = 3,

    /// <summary>
    /// Filters for dates that are greater than the specified date.
    /// Option Key: greaterThan
    /// Included by Default: Yes
    /// </summary>
    GreaterThan = 4,

    /// <summary>
    /// Filters for dates that are within a specified range.
    /// Option Key: inRange
    /// Included by Default: Yes
    /// </summary>
    InRange = 5,

    /// <summary>
    /// Filters for blank (null or empty) dates.
    /// Option Key: blank
    /// Included by Default: Yes
    /// </summary>
    Blank = 6,

    /// <summary>
    /// Filters for dates that are not blank (not null or empty).
    /// Option Key: notBlank
    /// Included by Default: Yes
    /// </summary>
    NotBlank = 7
}
