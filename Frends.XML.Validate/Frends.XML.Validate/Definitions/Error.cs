namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Error that occurred during the task.
/// </summary>
public class Error
{
    /// <summary>
    /// Summary of the error.
    /// </summary>
    /// <example>The element 'book' has invalid child element 'review'.</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional information about the error.
    /// </summary>
    /// <example>object { Exception? AdditionalInfo }</example>
    public Exception? AdditionalInfo { get; set; }
}
