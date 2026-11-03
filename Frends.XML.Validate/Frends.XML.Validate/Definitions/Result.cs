namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Validate task result.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Result of XML validation.
    /// </summary>
    /// <example>true</example>
    public bool IsValid { get; set; }

    /// <summary>
    /// Error that occurred during task execution or validation.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error? Error { get; set; }
}
