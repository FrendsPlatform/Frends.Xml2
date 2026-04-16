using Frends.XML.XPathQuery.Definitions;
using NUnit.Framework;
using System.IO;

namespace Frends.XML.XPathQuery.Tests;

[TestFixture]
public class XMLTests
{
    private const string Xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \r\n" +
        "<bookstore>\r\n" +
        "    <book genre=\"autobiography\" publicationdate=\"1981-03-22\" ISBN=\"1-861003-11-0\">\r\n" +
        "        <title>The Autobiography of Benjamin Franklin</title>\r\n" +
        "        <author>\r\n" +
        "            <first-name>Benjamin</first-name>\r\n" +
        "            <last-name>Franklin</last-name>\r\n" +
        "        </author>\r\n" +
        "        <price>8.99</price>\r\n" +
        "    </book>\r\n" +
        "    <book genre=\"novel\" publicationdate=\"1967-11-17\" ISBN=\"0-201-63361-2\">\r\n" +
        "        <title>The Confidence Man</title>\r\n" +
        "        <author>\r\n" +
        "            <first-name>Herman</first-name>\r\n" +
        "            <last-name>Melville</last-name>\r\n" +
        "        </author>\r\n" +
        "        <price>11.99</price>\r\n" +
        "    </book>\r\n" +
        "    <book genre=\"philosophy\" publicationdate=\"1991-02-15\" ISBN=\"1-861001-57-6\">\r\n" +
        "        <title>The Gorgias</title>\r\n" +
        "        <author>\r\n" +
        "            <name>Plato</name>\r\n" +
        "        </author>\r\n" +
        "        <price>9.99</price>\r\n" +
        "    </book>\r\n" +
        "</bookstore>\r\n";

    [Test]
    public void TestXPathQueryNoResultsFoundDontThrow()
    {
        const string xPath = "/bookstore/book/price/things";
        var res = XML.XPathQuery(
            new Input() { XML = Xml, XPathQuery = xPath },
            new Options() { ThrowErrorOnEmptyResults = false, ReturnRawXmlForNonAtomicValues = false });

        Assert.AreEqual(0, res.Data.Count);
    }

    [Test]
    public void TestXPathQueryNoResultsFoundThrow()
    {
        const string xPath = "/bookstore/book/price/things";
        var exception = Assert.Throws<InvalidDataException>(() =>
            XML.XPathQuery(
                new Input() { XML = Xml, XPathQuery = xPath },
                new Options() { ThrowErrorOnEmptyResults = true, ReturnRawXmlForNonAtomicValues = false }));

        Assert.IsTrue(exception?.Message.Contains("Could not find any nodes with XPath"));
    }

    [Test]
    public void TestXPathQueryResultsThatIsAtomic()
    {
        const string xPath = "/bookstore/book/price/string()";
        var res = XML.XPathQuery(
            new Input() { XML = Xml, XPathQuery = xPath },
            new Options());

        Assert.AreEqual(3, res.Data.Count);
        Assert.AreEqual("8.99", res.Data[0]);
        Assert.AreEqual("11.99", res.Data[1]);
        Assert.AreEqual("9.99", res.Data[2]);
    }

    [Test]
    public void TestXPathQueryResultsTextIsAtomic()
    {
        const string xPath = "/bookstore/book[1]/price/text()";
        var res = XML.XPathQuery(
            new Input() { XML = Xml, XPathQuery = xPath },
            new Options());

        Assert.AreEqual("8.99", res.Data[0]);
    }

    [Test]
    public void TestXPathQueryResultsThatIsAtomicAndCastToDouble()
    {
        const string xPath = "xs:double(/bookstore/book[1]/price/text())";
        var res = XML.XPathQuery(
            new Input() { XML = Xml, XPathQuery = xPath },
            new Options());

        Assert.AreEqual(8.99d, res.Data[0], 0.001d);
    }

    [Test]
    public void TestXPathQueryResultsThatIsOfTypeXml()
    {
        const string xPath = "/bookstore/book/price";
        var res = XML.XPathQuery(
            new Input() { XML = Xml, XPathQuery = xPath },
            new Options());

        Assert.AreEqual(8.99d, (double)res.Data[0].price);
        Assert.AreEqual(11.99d, (double)res.Data[1].price);
        Assert.AreEqual(9.99d, (double)res.Data[2].price);
    }

    [Test]
    public void TestXPathQueryWithRawXmlResult()
    {
        const string xPath = "/bookstore/book/price";
        var res = XML.XPathQuery(
            new Input() { XML = Xml, XPathQuery = xPath },
            new Options() { ThrowErrorOnEmptyResults = true, ReturnRawXmlForNonAtomicValues = true });

        Assert.AreEqual(3, res.Data.Count);
        Assert.AreEqual("<price>8.99</price>", res.Data[0]);
        Assert.AreEqual("<price>11.99</price>", res.Data[1]);
        Assert.AreEqual("<price>9.99</price>", res.Data[2]);
    }

    [Test]
    public void TestSchemaAwareQuery()
    {
        var xml = File.ReadAllText("TestXmls/SchemaAware.xml");
        const string xPath = "/*[local-name()='FeatureCollection']/@numberReturned/string()";
        var res = XML.XPathQuery(
            new Input { XML = xml, XPathQuery = xPath },
            new Options
            {
                ThrowErrorOnEmptyResults = true,
                ReturnRawXmlForNonAtomicValues = true,
                SchemaAware = true,
                SchemaValidationMode = SchemaValidationMode.None
            });

        Assert.AreEqual(1, res.Data.Count);
        Assert.AreEqual("5", res.Data[0]);
    }

    [Test]
    public void ShouldEnableXxeAttack()
    {
        const string xPath = "/stockCheck/productId";
        var secretPath = Path.GetFullPath("TestXmls/secret");

        var xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE foo [ <!ENTITY xxe SYSTEM ""file://{secretPath}""> ]>
<stockCheck><productId>&xxe;</productId></stockCheck>";
        var res = XML.XPathQuery(
            new Input
            {
                XML = xml,
                XPathQuery = xPath,
            },
            new Options
            {
                ThrowErrorOnEmptyResults = true,
                ReturnRawXmlForNonAtomicValues = true,
                SchemaAware = true,
                SchemaValidationMode = SchemaValidationMode.None,
                EnableExternalEntities = true,
            });

        Assert.That(res.Data[0] is string result && result.Contains("I LOVE DONUTS"), Is.True, res.Data[0]);
    }

    [Test]
    public void ShouldBlockXxeAttack()
    {
        const string xPath = "/stockCheck/productId";
        var secretPath = Path.GetFullPath("TestXmls/secret");

        var xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE foo [ <!ENTITY xxe SYSTEM ""file://{secretPath}""> ]>
<stockCheck><productId>&xxe;</productId></stockCheck>";
        var res = XML.XPathQuery(
            new Input
            {
                XML = xml,
                XPathQuery = xPath,
            },
            new Options
            {
                ThrowErrorOnEmptyResults = true,
                ReturnRawXmlForNonAtomicValues = true,
                SchemaAware = true,
                SchemaValidationMode = SchemaValidationMode.None,
                EnableExternalEntities = false,
            });

        Assert.That(res.Data[0] is string result && result.Contains("I LOVE DONUTS"), Is.False, res.Data[0]);
    }
}