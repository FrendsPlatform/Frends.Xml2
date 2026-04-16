using Frends.XSLT.Transform.Definitions;
using NUnit.Framework;
using System;
using System.IO;

namespace Frends.XSLT.Transform.Tests;

[TestFixture]
public class Tests
{
    [Test]
    public void TestXsltTransform()
    {
        const string transformXml = @"<?xml version=""1.0""?>
                                <hello-world>   <greeter>An XSLT Programmer</greeter>   <greeting>Hello, World!</greeting></hello-world>";

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
                                  <xsl:template match=""/hello-world"">
                                       <xsl:value-of select=""greeting""/> <xsl:apply-templates select=""greeter""/>
                                  </xsl:template>
                                  <xsl:template match=""greeter"">
                                    <DIV>from <I><xsl:value-of select="".""/></I></DIV>
                                  </xsl:template>
                                </xsl:stylesheet>";

        var res = XSLT.Transform(new Input { XML = transformXml, XSLT = xslt }, new Options { EnableExternalEntities = false });
        Assert.AreEqual(
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>Hello, World!<DIV>from <I>An XSLT Programmer</I></DIV>",
            res.XML);
    }

    [Test]
     public void TestXsltTransformWithNonUTF8Declaration()
    {
        const string transformXml = @"<?xml version=""1.0"" encoding=""Windows-1252"" ?><in>Ä</in>";

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
                                <xsl:output method=""xml"" />
                                  <xsl:template match=""/in"">
                                       <out><xsl:value-of select="".""/></out>
                                  </xsl:template>
                                </xsl:stylesheet>";

        var res = XSLT.Transform(new Input { XML = transformXml, XSLT = xslt }, new Options { EnableExternalEntities = false });
        Assert.AreEqual(
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?><out>Ä</out>",
            res.XML);
    }

    [Test]
    public void TestXsltTransformWithWhiteSpaces()
    {
        string transformXml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><in>foo  {0}{0}bar</in>", Environment.NewLine);

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
                                <xsl:output method=""xml"" />
                                  <xsl:template match=""/in"">
                                       <out><xsl:value-of select="".""/></out>
                                  </xsl:template>
                                </xsl:stylesheet>";

        var res = XSLT.Transform(new Input { XML = transformXml, XSLT = xslt }, new Options { EnableExternalEntities = false });

        Assert.AreEqual(
            string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><out>foo  {0}{0}bar</out>", Environment.NewLine),
            res.XML);
    }

    [Test]
    public void TestXsltTransformWithParams()
    {
        string transformXml = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><in />";

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
                                <xsl:output method=""xml"" />
                                <xsl:param name=""param"" />
                                  <xsl:template match=""/in"">
                                       <out><xsl:value-of select=""$param""/></out>
                                  </xsl:template>
                                </xsl:stylesheet>";

        var res = XSLT.Transform(
            new Input
            {
                XML = transformXml,
                XSLT = xslt,
                XSLTParameters = new XSLTParameter[] { new XSLTParameter { Name = "param", Value = "foo" } }
            }, new Options { EnableExternalEntities = false });

        Assert.AreEqual(
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?><out>foo</out>",
            res.XML);
    }


    [Test]
    public void TestXsltTransformWithNullParam()
    {
        string transformXml = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><in />";

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
                                <xsl:output method=""xml"" />
                                <xsl:param name=""param"" />
                                  <xsl:template match=""/in"">
                                       <out><xsl:value-of select=""$param""/></out>
                                  </xsl:template>
                                </xsl:stylesheet>";

        var res = XSLT.Transform(
            new Input
            {
                XML = transformXml,
                XSLT = xslt,
                XSLTParameters = new XSLTParameter[] { new()
                    { Name = "param", Value = null } },
            }, new Options { EnableExternalEntities = false });

        Assert.AreEqual(
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?><out/>",
            res.XML);
    }

    [Test]
    public void WhiteSpaceIsPreserved()
    {
        const string transformXml = @"<?xml version=""1.0""?>
                                <test></test>";

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
                                  <xsl:template match=""test"">
                                    <xsl:value-of select="".""/>
                                  </xsl:template>
                                </xsl:stylesheet>";

        var res = XSLT.Transform(new Input { XML = transformXml, XSLT = xslt }, new Options { EnableExternalEntities = false });

        Assert.AreEqual( // no newline caused by empty tag
            @"<?xml version=""1.0"" encoding=""UTF-8""?>",
            res.XML);
    }


    [Test]
    public void Transform_ShouldNotResolveExternalEntities()
    {
        const string xmlWithExternalEntity = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                                <!DOCTYPE root [
                                  <!ENTITY ext SYSTEM ""file:///etc/passwd"">
                                ]>
                                <root>&ext;</root>";

        const string xslt = @"<?xml version=""1.0""?>
                                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
                                  <xsl:template match=""root"">
                                    <xsl:value-of select=""."" />
                                  </xsl:template>
                                </xsl:stylesheet>";

        var input = new Input
        {
            XML = xmlWithExternalEntity,
            XSLT = xslt
        };

        var options = new Options
        {
            EnableExternalEntities = false
        };

        var result = XSLT.Transform(input, options);

        Assert.IsFalse(result.XML?.Contains("/etc/passwd"), "The external entity should not resolve to its system value.");
    }

    [Test]
    public void Transform_ShouldResolveExternalEntities()
    {
        var tempFilePath = Path.Combine(Path.GetTempPath(), "externalEntityFile.txt");
        var externalEntityContent = "This is the content of the external entity file.";
        File.WriteAllText(tempFilePath, externalEntityContent);

        string xmlWithExternalEntityReference = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                                <!DOCTYPE root [
                                              <!ENTITY ext SYSTEM ""file:///" + tempFilePath.Replace("\\", "/") + @""">
                                            ]>
                                            <root>&ext;</root>";

        const string xslt = @"<?xml version=""1.0""?>
                          <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
                            <xsl:template match=""root"">
                              <xsl:value-of select=""."" />
                            </xsl:template>
                          </xsl:stylesheet>";

        var input = new Input
        {
            XML = xmlWithExternalEntityReference,
            XSLT = xslt,
        };

        var options = new Options
        {
            EnableExternalEntities = true
        };

        var result = XSLT.Transform(input, options);

        Assert.IsTrue(result.XML?.Contains(externalEntityContent), "The external entity content should be included in the transformed XML.");

        File.Delete(tempFilePath);
    }

    [Test]
    public void Transform_ShouldNotLockExternalDocument()
    {
        var externalFilePath = Path.Combine(Path.GetTempPath(), "external_to_lock.xml");
        File.WriteAllText(externalFilePath, "<external>data</external>");

        string xsltWithDocument = $@"<?xml version=""1.0""?>
        <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
            <xsl:template match=""/"">
                <out>
                    <xsl:copy-of select=""document('{externalFilePath.Replace("\\", "/")}')"" />
                </out>
            </xsl:template>
        </xsl:stylesheet>";

        const string inputXml = "<root/>";

        var result = XSLT.Transform(
            new Input { XML = inputXml, XSLT = xsltWithDocument },
            new Options { EnableExternalEntities = true }
        );

        Assert.DoesNotThrow(() => {
            using (FileStream fs = new FileStream(externalFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                fs.WriteByte(0);
            }
        }, "file blocked");

        if (File.Exists(externalFilePath)) File.Delete(externalFilePath);
    }

    [Test]
    public void Transform_ShouldFallbackToDefaultResolver_WhenCustomLogicReturnsNull()
    {
        var fileName = "fallback_test.xml";
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        File.WriteAllText(fullPath, "<fallback>works</fallback>");

        try
        {
            string xslt = $@"<?xml version=""1.0""?>
        <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""2.0"">
            <xsl:template match=""/"">
                <out><xsl:copy-of select=""document('{fileName}')"" /></out>
            </xsl:template>
        </xsl:stylesheet>";

            var result = XSLT.Transform(
                new Input { XML = "<root/>", XSLT = xslt },
                new Options { EnableExternalEntities = true }
            );

            Assert.That(result.XML, Contains.Substring("<fallback>works</fallback>"));
        }
        finally
        {
            try { if (File.Exists(fullPath)) File.Delete(fullPath); } catch { }
        }
    }
}
