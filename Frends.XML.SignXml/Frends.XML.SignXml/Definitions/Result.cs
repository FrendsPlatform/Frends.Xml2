namespace Frends.XML.SignXml.Definitions;

/// <summary>
/// Result of the task.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// If output type is file, this will be a filepath.
    /// Otherwise, this will be the signed XML as string.
    /// </summary>
    /// <example>c:\temp\signedDocument.xml</example>
    public string Output { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
