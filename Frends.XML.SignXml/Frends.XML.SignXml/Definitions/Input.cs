namespace Frends.XML.SignXml.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class for SignXml-task.
/// </summary>
public class Input
{
    /// <summary>
    /// Input type.
    /// Possible types are File and XmlString.
    /// </summary>
    /// <example>File</example>
    public XmlParamType XmlInputType { get; set; }

    /// <summary>
    /// Path to XML document to sign.
    /// </summary>
    /// <example>c:\temp\document.xml</example>
    [DefaultValue("c:\\temp\\document.xml")]
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(XmlInputType), "", XmlParamType.File)]
    public string XmlFilePath { get; set; }

    /// <summary>
    /// XML to sign.
    /// </summary>
    /// <example><root><value>123</value></root></example>
    [DefaultValue("<root><value>123</value></root>")]
    [DisplayFormat(DataFormatString = "Xml")]
    [UIHint(nameof(XmlInputType), "", XmlParamType.XmlString)]
    public string Xml { get; set; }

    /// <summary>
    /// XML signing technique to use.
    /// </summary>
    /// <example>XmlEnvelopedSignature</example>
    public XmlEnvelopingType XmlEnvelopingType { get; set; }

    /// <summary>
    /// How to sign the document.
    /// </summary>
    /// <example>PrivateKeyCertificate</example>
    public SigningStrategyType SigningStrategy { get; set; }

    /// <summary>
    /// Path to certificate with private key.
    /// </summary>
    /// <example>c:\\certificates\\signingcertificate.pfx</example>
    [DefaultValue("c:\\certificates\\signingcertificate.pfx")]
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(SigningStrategy), "", SigningStrategyType.PrivateKeyCertificate)]
    public string CertificatePath { get; set; }

    /// <summary>
    /// Private key password.
    /// </summary>
    /// <example>password123</example>
    [PasswordPropertyText]
    [UIHint(nameof(SigningStrategy), "", SigningStrategyType.PrivateKeyCertificate)]
    public string PrivateKeyPassword { get; set; }

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
