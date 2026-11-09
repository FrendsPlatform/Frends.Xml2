using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.XSLT.Transform.Definitions;

/// <summary>
/// Transform task options.
/// </summary>
public class Options
{
    /// <summary>
    /// Controls whether external entities are allowed in the XML processing.
    /// </summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool EnableExternalEntities { get; set; } = false;

    /// <summary>
    /// Whether to throw an error on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Overrides the error message on failure.
    /// </summary>
    /// <example>XSLT transformation failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
