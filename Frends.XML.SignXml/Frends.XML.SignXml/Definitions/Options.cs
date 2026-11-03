namespace Frends.XML.SignXml.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Options class for SignXml-task.
/// </summary>
public class Options
{
    /// <summary>
    /// Switch to include comments.
    /// </summary>
    /// <example>true</example>
    public bool IncludeComments { get; set; }

    /// <summary>
    /// Should whitespace be preserved when loading the XML?
    /// </summary>
    /// <example>false</example>
    public bool PreserveWhitespace { get; set; }

    /// <summary>
    /// Signature methods to be used with signing.
    /// </summary>
    /// <example>RSASHA1</example>
    public XmlSignatureMethod XmlSignatureMethod { get; set; }

    /// <summary>
    /// Digest methods to be used.
    /// </summary>
    /// <example>SHA1</example>
    public DigestMethod DigestMethod { get; set; }

    /// <summary>
    /// Transform methods to be used.
    /// </summary>
    /// <example>[DsigC14, DsigC14WithComments]</example>
    public TransformMethod[] TransformMethods { get; set; }

    /// <summary>
    /// Whether to throw an error on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Overrides the error message on failure.
    /// </summary>
    /// <example>Failed to sign the XML document</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
