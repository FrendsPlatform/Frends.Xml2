namespace Frends.XML.XPathQuery.Definitions;

/// <summary>
/// Task result.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Data of the query.
    /// </summary>
    /// <example>[
    ///     "value123",
    ///     546,
    ///     true,
    ///     { "someObject": "hello" }
    /// ]</example>
    public dynamic Data { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
