using System.ComponentModel;
using System.Text;
using System.Xml;
using Frends.XML.XPathQuery.Definitions;
using Frends.XML.XPathQuery.Helpers;
using Newtonsoft.Json.Linq;
using Saxon.Api;
using LocalSchemaValidationMode = Frends.XML.XPathQuery.Definitions.SchemaValidationMode;
using SaxonSchemaValidationMode = Saxon.Api.SchemaValidationMode;

namespace Frends.XML.XPathQuery;

// We want to use all caps for abbreviations, but it is impossible to tell
// SonarLint about our exceptions (e.g. XSLT).
// Thus we disable this with pragma until SonarLint has an exception word list.
#pragma warning disable S101

/// <summary>
/// Frends task for querying XML with XPath.
/// </summary>
public static class XML
{
    static XML()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Queries XML for data using XPath query.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.XML.XPathQuery)
    /// </summary>
    /// <param name="input">Input definition.</param>
    /// <param name="options">Query options.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, List&lt;dynamic&gt; Data, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static Result XPathQuery(
        [PropertyTab] Input input,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var processor = Saxon.Activation.Activator.Activate();
            var xPathSelector = SetupXPathSelector(processor, input, options);
            var result = xPathSelector.Evaluate().GetList();

            if (options.ThrowErrorOnEmptyResults && !result.Any())
                throw new InvalidDataException($"Could not find any nodes with XPath: {input.XPathQuery}");

            return new Result
            {
                Success = true,
                Data = options.ReturnRawXmlForNonAtomicValues
                    ? result.Select(GetXmlOrAtomicObject).ToList()
                    : result.Select(GetJTokenFromXdmItem).ToList(),
                Error = null,
            };
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
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

    private static XPathSelector SetupXPathSelector(
        Processor processor,
        Input input,
        Options options)
    {
        var builder = processor.NewDocumentBuilder();
        builder.SchemaValidationMode = LocalToSaxonValidationMode(options.SchemaValidationMode);

        var xPathCompiler = processor.NewXPathCompiler();
        xPathCompiler.SchemaAware = options.SchemaAware;

        switch (options.XPathVersion)
        {
            case XPathVersion.V31:
                xPathCompiler.XPathLanguageVersion = "3.1";
                break;
            case XPathVersion.V30:
                xPathCompiler.XPathLanguageVersion = "3.0";
                break;
            case XPathVersion.V20:
                xPathCompiler.XPathLanguageVersion = "2.0";
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unsupported XPathVersion {options.XPathVersion}");
        }

        var xPathSelector = xPathCompiler.Compile(input.XPathQuery).Load();

        // Warning disabled because we cannot do anything else, since we are not handling a
        // real file from disk. It is all purely in memory.
#pragma warning disable S1075 // URIs should not be hardcoded
        builder.BaseUri = new Uri("file:///C:/example.txt");
#pragma warning restore S1075 // URIs should not be hardcoded

        using var stringReader = new StringReader(input.XML);
        var xmlReaderSettings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse,
            XmlResolver = options.EnableExternalEntities ? new XmlUrlResolver() : null,
        };
        using var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings);
        var xdmNode = builder.Build(xmlReader);
        xPathSelector.ContextItem = xdmNode;
        return xPathSelector;
    }

    private static SaxonSchemaValidationMode LocalToSaxonValidationMode(LocalSchemaValidationMode localMode)
    {
        switch (localMode)
        {
            case LocalSchemaValidationMode.None:
                return SaxonSchemaValidationMode.None;
            case LocalSchemaValidationMode.Lax:
                return SaxonSchemaValidationMode.Lax;
            case LocalSchemaValidationMode.Strict:
                return SaxonSchemaValidationMode.Strict;
        }

        throw new ArgumentOutOfRangeException($"Unsupported SchemaValidationMode {localMode}");
    }
}
