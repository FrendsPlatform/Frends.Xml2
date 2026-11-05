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
        private readonly string privateKeyPassword = "password";
        private string workingDirectory;
        private string certificatePath;

        [SetUp]
        public void Setup()
        {
            workingDirectory = Path.Combine(Path.GetTempPath(), "Frends.XML.SignXml.Tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(workingDirectory);
            certificatePath = Path.Combine(workingDirectory, "certwithpk.pfx");
            TestFiles.CreateSignatureFile.GenerateSignatureFile(certificatePath, privateKeyPassword);
        }

        [TearDown]
        public void Down()
        {
            if (Directory.Exists(workingDirectory))
            {
                Directory.Delete(workingDirectory, true);
            }
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

        [TestCase(XmlSignatureMethod.RSASHA1, DigestMethod.SHA1)]
        [TestCase(XmlSignatureMethod.RSASHA384, DigestMethod.SHA384)]
        [TestCase(XmlSignatureMethod.RSASHA512, DigestMethod.SHA512)]
        public void SignXml_ShouldSupportSignatureAndDigestAlgorithms(XmlSignatureMethod signatureMethod, DigestMethod digestMethod)
        {
            var input = new Input
            {
                CertificatePath = certificatePath,
                PrivateKeyPassword = privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlInputType = XmlParamType.XmlString,
                Xml = "<root><value>foo</value></root>",
                OutputType = XmlParamType.XmlString,
            };
            var options = new Options
            {
                DigestMethod = digestMethod,
                XmlSignatureMethod = signatureMethod,
            };

            var result = XML.SignXml(input, options, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            StringAssert.Contains("<Signature", result.Output);
        }

        [TestCase(TransformMethod.DsigBase64)]
        [TestCase(TransformMethod.DsigC14)]
        [TestCase(TransformMethod.DsigC14WithComments)]
        [TestCase(TransformMethod.DsigExcC14WithComments)]
        public void SignXml_ShouldSupportTransformMethods(TransformMethod transformMethod)
        {
            var input = new Input
            {
                CertificatePath = certificatePath,
                PrivateKeyPassword = privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlInputType = XmlParamType.XmlString,
                Xml = "<root><value>foo</value></root>",
                OutputType = XmlParamType.XmlString,
            };
            var options = new Options
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new[] { transformMethod },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256,
            };

            var result = XML.SignXml(input, options, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            StringAssert.Contains("<Signature", result.Output);
        }

        [Test]
        public void SignXml_ShouldReturnFailureWhenInvalidXmlAndThrowingIsDisabled()
        {
            var input = new Input
            {
                XmlInputType = XmlParamType.XmlString,
                Xml = " ",
                OutputType = XmlParamType.XmlString,
            };
            var options = new Options
            {
                ThrowErrorOnFailure = false,
            };

            var result = XML.SignXml(input, options, CancellationToken.None);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Message, Is.EqualTo("Invalid input xml"));
            Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public void SignXml_ShouldThrowWhenCancellationIsRequested()
        {
            var input = new Input
            {
                CertificatePath = certificatePath,
                PrivateKeyPassword = privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlInputType = XmlParamType.XmlString,
                Xml = "<root><value>foo</value></root>",
                OutputType = XmlParamType.XmlString,
            };
            var options = new Options
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new[] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256,
            };
            using var cancellationSource = new CancellationTokenSource();
            cancellationSource.Cancel();

            Assert.Throws<OperationCanceledException>(() => XML.SignXml(input, options, cancellationSource.Token));
        }

        [Test]
        public void SignXml_ShouldSignXmlFileWithPrivateKeyCertificate()
        {
            // Create file.
            var xmlFilePath = Path.Combine(workingDirectory, Guid.NewGuid().ToString() + ".xml");
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
            var xmlFilePath = Path.Combine(workingDirectory, Guid.NewGuid().ToString() + ".xml");
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
