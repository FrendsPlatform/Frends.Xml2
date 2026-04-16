using Frends.Xml.Validate.Definitions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Frends.Xml.Validate.Tests;

[TestFixture]
public class Tests
{
   private const string SimpleXsd = @"<xsd:schema xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                                                targetNamespace=""urn:books""
                                                xmlns:bks=""urn:books"">

                                      <xsd:element name=""books"" type=""bks:BooksForm""/>

                                      <xsd:complexType name=""BooksForm"">
                                        <xsd:sequence>
                                          <xsd:element name=""book"" 
                                                      type=""bks:BookForm"" 
                                                      minOccurs=""0"" 
                                                      maxOccurs=""unbounded""/>
                                          </xsd:sequence>
                                      </xsd:complexType>

                                      <xsd:complexType name=""BookForm"">
                                        <xsd:sequence>
                                          <xsd:element name=""author""   type=""xsd:string""/>
                                          <xsd:element name=""title""    type=""xsd:string""/>
                                          <xsd:element name=""genre""    type=""xsd:string""/>
                                          <xsd:element name=""price""    type=""xsd:float"" />
                                          <xsd:element name=""pub_date"" type=""xsd:date"" />
                                          <xsd:element name=""review""   type=""xsd:string""/>
                                        </xsd:sequence>
                                        <xsd:attribute name=""id""   type=""xsd:string""/>
                                      </xsd:complexType>
                                    </xsd:schema>";

   [Test]
   public void TestValidationFailWithMessage()
   {
      const string simpleXml = @"<?xml version=""1.0""?>
                                    <x:books xmlns:x=""urn:books"">
                                       <book id=""bk001"">
                                          <author>Writer</author>
                                          <title>The First Book</title>
                                          <genre>Fiction</genre>
                                          <price>44.95</price>
                                          <pub_date>2000-10-01</pub_date>
                                          <review>An amazing story of nothing.</review>
                                       </book>

                                       <book id=""bk002"">
                                          <author>Poet</author>
                                          <title>The Poet's First Poem</title>
                                          <genre>Poem</genre>
                                          <price>24.95</price>
                                          <review>Least poetic poems.</review>
                                       </book>
                                    </x:books>";


      var result = Xml.Validate(
          new Input { Xml = simpleXml, XsdSchemas = new[] { SimpleXsd } },
          new Options());
      ClassicAssert.IsFalse(result.IsValid);
      ClassicAssert.IsTrue(result.Error != null &&
                    result.Error.Contains("The element 'book' has invalid child element 'review'."));
   }

   [Test]
   public void TestValidationSucceeds()
   {
      const string simpleXml = @"<?xml version=""1.0""?>
                                    <x:books xmlns:x=""urn:books"">
                                       <book id=""bk001"">
                                          <author>Writer</author>
                                          <title>The First Book</title>
                                          <genre>Fiction</genre>
                                          <price>44.95</price>
                                          <pub_date>2000-10-01</pub_date>
                                          <review>An amazing story of nothing.</review>
                                       </book>

                                       <book id=""bk002"">
                                          <author>Poet</author>
                                          <title>The Poet's First Poem</title>
                                          <genre>Poem</genre>
                                          <price>24.95</price>
                                          <pub_date>2000-10-01</pub_date>
                                          <review>Least poetic poems.</review>
                                       </book>
                                    </x:books>";


      var result = Xml.Validate(
          new Input { Xml = simpleXml, XsdSchemas = new[] { SimpleXsd } },
          new Options());
      ClassicAssert.IsTrue(result.IsValid);
   }
}