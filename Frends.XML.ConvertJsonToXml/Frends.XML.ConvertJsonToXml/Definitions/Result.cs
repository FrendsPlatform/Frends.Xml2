namespace Frends.XML.ConvertJsonToXml.Definitions;

/// <summary>
/// Task's result class.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Xml string.
    /// </summary>
    /// <example>&lt;root&gt;&lt;/root&gt;</example>
    public string Xml { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
