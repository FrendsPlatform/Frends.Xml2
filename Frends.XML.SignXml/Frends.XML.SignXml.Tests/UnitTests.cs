namespace Frends.XML.SignXml.Tests;

using Frends.XML.SignXml.Definitions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

[TestFixture]
internal class UnitTests
{
    [TestFixture]
#if !_WINDOWS
    [Ignore("No .pfx file")] // Signature creation not working in Linux.
#endif
    public class SigningTaskTest
    {
        private readonly string certificatePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "TestFiles", "certwithpk.pfx");
        private readonly string privateKeyPassword = "password";

        [SetUp]
        public void Setup()
        {
            TestFiles.CreateSignatureFile.GenerateSignatureFile(certificatePath, privateKeyPassword);
        }

        [TearDown]
        public void Down()
        {
            File.Delete(certificatePath);
        }

        [Test]
        public void SignXml_ShouldSignXmlStringWithPrivateKeyCertificate()
        {
            var input = new Input
            {
                CertificatePath = certificatePath,
                PrivateKeyPassword = privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlInputType = XmlParamType.XmlString,
                XmlEnvelopingType = XmlEnvelopingType.XmlEnvelopedSignature,
                Xml = "<root><value>foo</value></root>",
                OutputType = XmlParamType.XmlString,
            };
            var options = new Options
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new[] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256,
            };

            var result = XML.SignXml(input, options, CancellationToken.None);

            StringAssert.Contains("<Signature", result.Output);
        }

        [Test]
        public void SignXml_ShouldSignXmlFileWithPrivateKeyCertificate()
        {
            // Create file.
            var xmlFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "TestFiles", Guid.NewGuid().ToString() + ".xml");
            File.WriteAllText(xmlFilePath, @"<root><value>foo</value></root>");
            var input = new Input
            {
                CertificatePath = certificatePath,
                PrivateKeyPassword = privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlEnvelopingType = XmlEnvelopingType.XmlEnvelopedSignature,
                XmlInputType = XmlParamType.File,
                XmlFilePath = xmlFilePath,
                OutputType = XmlParamType.File,
                OutputFilePath = xmlFilePath.Replace(".xml", "_signed.xml"),
                OutputEncoding = "utf-8",
            };
            var options = new Options
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new[] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256,
                PreserveWhitespace = true,
            };

            var result = XML.SignXml(input, options, CancellationToken.None);
            var signedXml = File.ReadAllText(result.Output);

            StringAssert.Contains("<Signature", signedXml);
            StringAssert.DoesNotContain("<Signature", File.ReadAllText(xmlFilePath));

            // Cleanup.
            File.Delete(xmlFilePath);
            File.Delete(result.Output);
        }

        [Test]
        public void SignXml_ShouldAddSignatureToSourceFile()
        {
            // Create file.
            var xmlFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "TestFiles", Guid.NewGuid().ToString() + ".xml");
            File.WriteAllText(xmlFilePath, @"<root><value>foo</value></root>");
            var input = new Input
            {
                CertificatePath = certificatePath,
                PrivateKeyPassword = privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlEnvelopingType = XmlEnvelopingType.XmlEnvelopedSignature,
                XmlInputType = XmlParamType.File,
                XmlFilePath = xmlFilePath,
                OutputType = XmlParamType.File,
                AddSignatureToSourceFile = true,
            };
            var options = new Options
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new[] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256,
                PreserveWhitespace = true,
            };

            var result = XML.SignXml(input, options, CancellationToken.None);
            var signedXml = File.ReadAllText(result.Output);

            StringAssert.Contains("<Signature", signedXml);

            // Cleanup.
            File.Delete(xmlFilePath);
            File.Delete(result.Output);
        }
    }
}
