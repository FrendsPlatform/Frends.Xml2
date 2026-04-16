namespace Frends.XML.ConvertJsonToXml;

using System.ComponentModel;
using Frends.XML.ConvertJsonToXml.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class XML
{
    /// <summary>
    /// Frends Task for converting Json string to Xml string.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.XML.ConvertJsonToXml).
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <returns>Object { string Xml }</returns>
    public static Result ConvertJsonToXml([PropertyTab] Input input)
    {
        JToken declaration = null;
        var jObject = JObject.Parse(input.Json);

        if (jObject.ContainsKey("?xml"))
        {
            declaration = jObject["?xml"];
            jObject.Remove("?xml");
        }

        var doc = JsonConvert.DeserializeXmlNode(jObject.ToString(), input.XmlRootElementName);

        if (input.IncludeXmlDeclaration)
        {
            var root = doc.DocumentElement;

            if (declaration != null)
            {
                var xmlDeclaration = doc.CreateXmlDeclaration(
                    declaration["@version"]?.ToString(),
                    declaration["@encoding"]?.ToString(),
                    declaration["@standalone"]?.ToString());
                doc.InsertBefore(xmlDeclaration, root);
            }
            else
            {
                doc.InsertBefore(doc.CreateXmlDeclaration("1.0", null, null), root);
            }
        }

        return new Result(doc.OuterXml);
    }
}
