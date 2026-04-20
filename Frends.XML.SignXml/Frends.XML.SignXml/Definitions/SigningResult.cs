namespace Frends.XML.SignXml.Definitions;

/// <summary>
/// Output class for SignXml-task.
/// </summary>
public class SigningResult
{
    /// <summary>
    /// If output type is file, this will be a filepath.
    /// Otherwise, this will be the signed XML as string.
    /// </summary>
    /// <example>c:\temp\signedDocument.xml</example>
    public string Result { get; set; }
}
