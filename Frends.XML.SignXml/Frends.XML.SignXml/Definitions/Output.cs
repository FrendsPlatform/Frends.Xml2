namespace Frends.XML.SignXml.Definitions;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

/// <summary>
/// Output class for SignXml-task.
/// </summary>
public class Output
{
    /// <summary>
    /// Output to file or XML string?
    /// </summary>
    /// <example>File</example>
    public XmlParamType OutputType { get; set; }

    /// <summary>
    /// A filepath for the output XML.
    /// </summary>
    /// <example>c:\\temp\\signedOutput.xml</example>
    [DefaultValue("c:\\temp\\signedOutput.xml")]
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(OutputType), "", XmlParamType.File)]
    public string OutputFilePath { get; set; }

    /// <summary>
    /// The encoding for the output file.
    /// </summary>
    /// <example>UTF-8</example>
    [DefaultValue("UTF-8")]
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(OutputType), "", XmlParamType.File)]
    public string OutputEncoding { get; set; }

    /// <summary>
    /// If source is file, then you can add signature to it.
    /// </summary>
    /// <example>true</example>
    [UIHint(nameof(OutputType), "", XmlParamType.File)]
    public bool AddSignatureToSourceFile { get; set; }
}
