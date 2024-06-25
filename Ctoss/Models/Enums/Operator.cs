namespace Ctoss.Models.Enums;

/// <summary>
/// Specifies the logical operators for combining filter conditions.
/// </summary>
public enum Operator
{
    /// <summary>
    /// No logical operator applied.
    /// </summary>
    NoOp = 0,

    /// <summary>
    /// Logical AND operator. Both conditions must be true.
    /// Option Key: and
    /// </summary>
    And = 1,

    /// <summary>
    /// Logical OR operator. At least one condition must be true.
    /// Option Key: or
    /// </summary>
    Or = 2
}
