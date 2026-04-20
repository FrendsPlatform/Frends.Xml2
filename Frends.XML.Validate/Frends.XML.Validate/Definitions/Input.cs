namespace Frends.Xml.Validate.Definitions;

/// <summary>
/// Validate task input.
/// </summary>
public class Input
{
    /// <summary>
    /// XML to validate.
    /// </summary>
    /// <example>&lt;xml&gt;content&lt;/xml&gt;</example>
    public string Xml { get; set; } = "";

    /// <summary>
    /// List of XML Schema Definitions
    /// </summary>
    /// <example>[ "&lt;xs:schema&gt;...&lt;/xs:schema&gt;", "&amp;lt;xs:schema&amp;gt;...&amp;lt;/xs:schema&amp;gt;" ]</example>
    public string[] XsdSchemas { get; set; } = Array.Empty<string>();
}