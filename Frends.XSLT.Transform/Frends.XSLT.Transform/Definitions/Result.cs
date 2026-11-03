namespace Frends.XSLT.Transform.Definitions;

/// <summary>
/// XSLT Transform task result.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Result of XSLT transformation.
    /// </summary>
    /// <example>&lt;result&gt;foo&lt;/result&gt;</example>
    public string XML { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
