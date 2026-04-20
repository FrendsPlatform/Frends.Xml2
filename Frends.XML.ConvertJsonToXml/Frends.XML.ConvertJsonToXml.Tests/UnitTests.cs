namespace Frends.XML.ConvertJsonToXml.Tests;

using Newtonsoft.Json;
using Frends.XML.ConvertJsonToXml.Definitions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

[TestFixture]
internal class UnitTests
{
    private string _json;
    private string _complexJson;
    private Input _input;

    [SetUp]
    public void Setup()
    {
        _json = @"
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
        _complexJson = @"{
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

        _input = new Input()
        {
            Json = _json,
            XmlRootElementName = string.Empty,
            IncludeXmlDeclaration = true,
        };
    }

    [Test]
    public void ConvertJsonToXml()
    {
        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsTrue(result.Xml.Contains("<url>http://www.google.com</url>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<person id=""1"">"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<name>Alan</name>"));
    }

    [Test]
    public void ConvertJsonToXmlWithNewRootElement()
    {
        _input.XmlRootElementName = "newRoot";
        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsTrue(result.Xml.Contains("<url>http://www.google.com</url>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithoutXmlDeclaration()
    {
        _input.IncludeXmlDeclaration = false;
        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithXmlDeclarationNoNewRoot()
    {
        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithoutXmlDeclarationAndWithNewRootElement()
    {
        _input.IncludeXmlDeclaration = false;
        _input.XmlRootElementName = "newRoot";
        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
        ClassicAssert.IsTrue(result.Xml.StartsWith("<newRoot>"));
    }

    [Test]
    public void ConvertJsonToXmlWithSimpleXml()
    {
        var json = @"{""name"": ""Matt"", ""address"": ""Street 1""}";
        _input.Json = json;
        _input.XmlRootElementName = "root";
        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsTrue(result.Xml.Contains("<name>Matt</name>"));
        ClassicAssert.IsTrue(result.Xml.Contains("<address>Street 1</address>"));
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithSimpleXmlWithoutNewRootElementShouldThrow()
    {
        var json = @"{""name"": ""Matt"", ""address"": ""Street 1""}";
        _input.Json = json;
        var ex = Assert.Throws<JsonSerializationException>(() => XML.ConvertJsonToXml(_input));
        ClassicAssert.AreEqual("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifying a DeserializeRootElementName. Path 'address', line 3, position 12.", ex.Message);
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJson()
    {
        _input.Json = _complexJson;

        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJsonWithNewRootElement()
    {
        _input.XmlRootElementName = "root";
        _input.Json = _complexJson;

        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsTrue(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJsonWithoutDeclaration()
    {
        _input.Json = _complexJson;
        _input.IncludeXmlDeclaration = false;

        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }

    [Test]
    public void ConvertJsonToXmlWithComplexJsonWithNewRootElementWithoutDeclaration()
    {
        _input.XmlRootElementName = "root";
        _input.Json = _complexJson;
        _input.IncludeXmlDeclaration = false;

        var result = XML.ConvertJsonToXml(_input);
        ClassicAssert.IsFalse(result.Xml.Contains(@"<?xml version=""1.0"" standalone=""no""?>"));
    }
}
