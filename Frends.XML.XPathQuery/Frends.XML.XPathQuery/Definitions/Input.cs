namespace Frends.XML.XPathQuery.Definitions;

/// <summary>
/// Query task input.
/// </summary>
public class Input
{
    /// <summary>
    /// XML to be queried.
    /// </summary>
    /// <example>&lt;book&gt;&lt;title&gt;Everyday Italian&lt;/title&gt;&lt;/book&gt;</example>
    public string XML { get; set; } = string.Empty;

    /// <summary>
    /// The XPath Query
    /// </summary>
    /// <example>/book/title/text()</example>
    public string XPathQuery { get; set; } = string.Empty;
}
