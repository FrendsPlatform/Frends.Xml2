namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Validate task input.
/// </summary>
public class Options
{
    /// <summary>
    /// Whether to throw and exception on validation error or return as result.
    /// </summary>
    /// <example>true</example>
    public bool ThrowOnValidationErrors { get; set; }
}