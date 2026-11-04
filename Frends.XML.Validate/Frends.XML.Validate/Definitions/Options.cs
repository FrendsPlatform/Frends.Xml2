using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Validate task options.
/// </summary>
public class Options
{
    /// <summary>
    /// Controls how XML validation errors are handled. When false, validation errors are returned in the result
    /// with Success and IsValid set to false, regardless of ThrowErrorOnFailure. When true, a validation error
    /// becomes an exception; ThrowErrorOnFailure then controls whether that exception is thrown or returned
    /// as a failed result.
    /// </summary>
    /// <example>false</example>
    public bool ThrowOnValidationErrors { get; set; }

    /// <summary>
    /// Controls whether task exceptions are thrown or returned in a failed result. This applies to validation
    /// errors only when ThrowOnValidationErrors is true. When ThrowOnValidationErrors is false, validation
    /// errors are returned in the result without throwing, regardless of this option.
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
