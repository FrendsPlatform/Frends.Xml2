namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Validate task result.
/// </summary>
public class Result
{
    /// <summary>
    /// Result of XML validation.
    /// </summary>
    /// <example>true</example>
    public bool IsValid { get; internal set; }

    /// <summary>
    /// Validation error message, if any.
    /// </summary>
    /// <example>The element 'book' has invalid child element 'review'.
    /// </example>
    public string? Error { get; internal set; }
}