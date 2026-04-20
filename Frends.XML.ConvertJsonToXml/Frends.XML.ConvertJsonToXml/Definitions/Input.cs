namespace Frends.XML.ConvertJsonToXml.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class for converting JSON string to XML string.
/// </summary>
public class Input
{
    /// <summary>
    /// Json string to be converted to XML.
    /// </summary>
    /// <example>
    /// <code>
    /// {
    ///     '?xml': {
    ///     '@version': '1.0',
    ///     '@standalone': 'no'
    ///     },
    ///     'root': {
    ///         'person': [
    ///             {
    ///                 '@id': '1',
    ///                 'name': 'Alan',
    ///                 'url': 'http://www.google.com'
    ///             }
    ///         ]
    ///     }
    /// }
    /// </code>
    /// </example>
    [DisplayFormat(DataFormatString = "Json")]
    public string Json { get; set; }

    /// <summary>
    /// The name for the root XML element.
    /// </summary>
    /// <example>root</example>
    public string XmlRootElementName { get; set; }

    /// <summary>
    /// Enables the Xml declaration to be included to the Xml string.
    /// Doesn't affect to the xml when XmlRootElementName is empty.
    /// Xml declaration needs to be in the json otherwise default xml declaration is used.
    /// &lt;?xml version="1.0"?&gt;
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool IncludeXmlDeclaration { get; set; }
}