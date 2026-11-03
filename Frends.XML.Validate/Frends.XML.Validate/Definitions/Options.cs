using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Validate task options.
/// </summary>
public class Options
{
    /// <summary>
    /// Whether to throw and exception on validation error or return as result.
    /// </summary>
    /// <example>true</example>
    public bool ThrowOnValidationErrors { get; set; }

    /// <summary>
    /// Whether to throw an error on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Overrides the error message on failure.
    /// </summary>
    /// <example>XML validation failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
