using Newtonsoft.Json.Linq;
using Saxon.Api;
using System.Xml;

namespace Frends.XML.XPathQuery.Definitions;

/// <summary>
/// Task result.
/// </summary>
public class Result
{
    internal Result(IEnumerable<XdmItem> xdmItems, bool raw = false)
    {
        Data = raw 
            ? new Lazy<List<object>>(() => xdmItems.Select(GetXmlOrAtomicObject).ToList()).Value
            : new Lazy<List<object>>(() => xdmItems.Select(GetJTokenFromXdmItem).ToList()).Value;
    }

    private static object GetXmlOrAtomicObject(XdmItem item)
    {
        return item is XdmAtomicValue xdmAtomicValue ? xdmAtomicValue.Value : item.ToString();
    }

    private static object GetJTokenFromXdmItem(XdmItem xdmItem)
    {
        if (xdmItem is XdmAtomicValue xdmAtomicValue)
            return xdmAtomicValue.Value;

        // Saxon does not treat "text()" results as atomic types, so we need to manually
        // check for text nodes. This makes queries like /something/text() work properly.
        if (xdmItem is XdmNode xdmNode && xdmNode.NodeType == XmlNodeType.Text)
            return xdmNode.StringValue;

        var output = new XmlDocument();
        output.LoadXml(xdmItem.ToString());
        return JToken.FromObject(output);
    }

    /// <summary>
    /// Data of the query.
    /// </summary>
    /// <example>[
    ///     "value123",
    ///     546,
    ///     true,
    ///     { "someObject": "hello" }
    /// ]</example>
    public dynamic Data { get; private set; }
}
