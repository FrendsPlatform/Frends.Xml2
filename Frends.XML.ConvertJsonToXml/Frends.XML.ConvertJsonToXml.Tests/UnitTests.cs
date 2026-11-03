namespace Frends.XML.ConvertJsonToXml.Tests;

using Frends.XML.ConvertJsonToXml.Definitions;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;

[TestFixture]
internal class UnitTests
{
    private string json;
    private string complexJson;
    private Input input;
    private Options options = new Options();

    [SetUp]
    public void Setup()
    {
        json = @"
{
    '?xml': {
        '@version': '1.0',
        '@standalone': 'no'
    },
    'root': {
        'person': [
        {
            '@id': '1',
            'name': 'Alan',
            'url': 'http://www.google.com'
        },
        {
            '@id': '2',
            'name': 'Louis',
            'url': 'http://www.yahoo.com'
        }
        ]
    }
}";
        complexJson = @"{
            ""?xml"": {
                ""@version"": ""1.0"",
                ""@standalone"": ""no""
            },
            ""glossary"": {
                ""title"": ""example glossary"",
		""GlossDiv"": {
                    ""title"": ""S"",
			""GlossList"": {
                        ""GlossEntry"": {
                            ""ID"": ""SGML"",
					""SortAs"": ""SGML"",
					""GlossTerm"": ""Standard Generalized Markup Language"",
					""Acronym"": ""SGML"",
					""Abbrev"": ""ISO 8879:1986"",
					""GlossDef"": {
                                ""para"": ""A meta-markup language, used to create markup languages such as DocBook."",
						""GlossSeeAlso"": [""GML"", ""XML""]
                    },
					""GlossSee"": ""markup""
                        }
                    }
                }
            }
        }";

        input = new Input()
        {
            Json = json,
            XmlRootElementName = string.Empty,
            IncludeXmlDeclaration = true,
        };
    }

    [Test]
    public void ConvertJsonToXml()
    {
        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsTrue(result.Xml.Contains("<url>http://www.google.com</url>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<person id=""1"">"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<name>Alan</name>"));
    }

    [Test]
    public void ConvertJsonToXmlWithNewRootElement()
    {
        input.XmlRootElementName = "newRoot";
        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsTrue(result.Xml.Contains("<url>http://www.google.com</url>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithoutXmlDeclaration()
    {
        input.IncludeXmlDeclaration = false;
        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithXmlDeclarationNoNewRoot()
    {
        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithoutXmlDeclarationAndWithNewRootElement()
    {
        input.IncludeXmlDeclaration = false;
        input.XmlRootElementName = "newRoot";
        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
        ClassicAssert.IsTrue(result.Xml.StartsWith("<newRoot>"));
    }

    [Test]
    public void ConvertJsonToXmlWithSimpleXml()
    {
        var json = @"{""name"": ""Matt"", ""address"": ""Street 1""}";
        input.Json = json;
        input.XmlRootElementName = "root";
        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsTrue(result.Xml.Contains("<name>Matt</name>"));
        ClassicAssert.IsTrue(result.Xml.Contains("<address>Street 1</address>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithSimpleXmlWithoutNewRootElementShouldThrow()
    {
        var json = @"{""name"": ""Matt"", ""address"": ""Street 1""}";
        input.Json = json;
        var ex = Assert.Throws<JsonSerializationException>(() => XML.ConvertJsonToXml(input, options, CancellationToken.None));
        ClassicAssert.AreEqual("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifying a DeserializeRootElementName. Path 'address', line 3, position 12.", ex.Message);
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJson()
    {
        input.Json = complexJson;

        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJsonWithNewRootElement()
    {
        input.XmlRootElementName = "root";
        input.Json = complexJson;

        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJsonWithoutDeclaration()
    {
        input.Json = complexJson;
        input.IncludeXmlDeclaration = false;

        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJsonWithNewRootElementWithoutDeclaration()
    {
        input.XmlRootElementName = "root";
        input.Json = complexJson;
        input.IncludeXmlDeclaration = false;

        var result = XML.ConvertJsonToXml(input, options, CancellationToken.None);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }
}
