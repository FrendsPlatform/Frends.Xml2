namespace Frends.XML.ConvertJsonToXml.Definitions;

/// <summary>
/// Task's result class.
/// </summary>
public class Result
{
    internal Result(string xml)
    {
        Xml = xml;
    }

    /// <summary>
    /// Xml string.
    /// </summary>
    /// <example>&lt;root&gt;&lt;/root&gt;</example>
    public string Xml { get; private set; }
}
