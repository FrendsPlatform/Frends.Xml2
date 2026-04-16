namespace Frends.XML.SignXml;

using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Xml;
using Frends.XML.SignXml.Definitions;

/// <summary>
/// Frends task for signing XML.
/// </summary>
public static class XML
{
    /// <summary>
    /// Sign an XML document.
    /// [Documentation](https://tasks.frends.com/tasks/Frends-XML-SignXml/)
    /// </summary>
    /// <param name="input">Parameters for input XML.</param>
    /// <param name="output">Parameters for output XML.</param>
    /// <param name="options">Options for the signing operation.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>SigningResult</returns>
    public static SigningResult SignXml([PropertyTab] Input input, [PropertyTab] Output output, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        var result = new SigningResult();
        var xmldoc = new XmlDocument() { PreserveWhitespace = options.PreserveWhitespace };
        StreamReader xmlStream = null;

        if (input.XmlInputType == XmlParamType.File)
        {
            xmlStream = new StreamReader(input.XmlFilePath);
            xmldoc.Load(xmlStream);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(input.Xml)) throw new ArgumentException("Invalid input xml");
            xmldoc.LoadXml(input.Xml);
        }

        var signedXml = new SignedXml(xmldoc);

        // Determine signature method.
        switch (options.XmlSignatureMethod)
        {
            case XmlSignatureMethod.RSASHA1:
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
                break;
            case XmlSignatureMethod.RSASHA256:
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
                break;
            case XmlSignatureMethod.RSASHA384:
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA384Url;
                break;
            case XmlSignatureMethod.RSASHA512:
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA512Url;
                break;
        }

        // Determine how to sign.
        switch (input.SigningStrategy)
        {
            case SigningStrategyType.PrivateKeyCertificate:
                var cert = new X509Certificate2(input.CertificatePath, input.PrivateKeyPassword, X509KeyStorageFlags.Exportable);
                signedXml.SigningKey = cert.GetRSAPrivateKey();

                // Public key certificate is submitted with the XML document.
                var keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(cert));
                signedXml.KeyInfo = keyInfo;
                break;
        }

        var reference = new Reference();
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(options.IncludeComments));

        // Add different transforms.
        foreach (var transform in options.TransformMethods)
        {
            cancellationToken.ThrowIfCancellationRequested();
            switch (transform)
            {
                case TransformMethod.DsigBase64:
                    reference.AddTransform(new XmlDsigBase64Transform());
                    break;
                case TransformMethod.DsigC14:
                    reference.AddTransform(new XmlDsigC14NTransform());
                    break;
                case TransformMethod.DsigC14WithComments:
                    reference.AddTransform(new XmlDsigC14NWithCommentsTransform());
                    break;
                case TransformMethod.DsigExcC14:
                    reference.AddTransform(new XmlDsigExcC14NTransform());
                    break;
                case TransformMethod.DsigExcC14WithComments:
                    reference.AddTransform(new XmlDsigExcC14NWithCommentsTransform());
                    break;
            }
        }

        // Target the whole XML document.
        reference.Uri = string.Empty;

        // Add digest method.
        switch (options.DigestMethod)
        {
            case DigestMethod.SHA1:
                reference.DigestMethod = SignedXml.XmlDsigSHA1Url;
                break;
            case DigestMethod.SHA256:
                reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
                break;
            case DigestMethod.SHA384:
                reference.DigestMethod = SignedXml.XmlDsigSHA384Url;
                break;
            case DigestMethod.SHA512:
                reference.DigestMethod = SignedXml.XmlDsigSHA512Url;
                break;
        }

        // Add references to signed XML.
        signedXml.AddReference(reference);

        // Compute the signature.
        signedXml.ComputeSignature();

        // As this is XML Enveloped Signature, add the signature element to the original XML as the last child of the root element.
        xmldoc.DocumentElement.AppendChild(xmldoc.ImportNode(signedXml.GetXml(), true));

        // Output results either to a file or result object.
        if (output.OutputType == XmlParamType.File)
        {
            xmlStream.Dispose();

            if (output.AddSignatureToSourceFile)
            {
                // Signed XML document is written in target destination.
                xmldoc.Save(input.XmlFilePath);

                // Result will indicate the source file path.
                result.Result = input.XmlFilePath;
            }
            else
            {
                // Signed XML document is written in target destination.
                using (var writer = new XmlTextWriter(output.OutputFilePath, Encoding.GetEncoding(output.OutputEncoding)))
                {
                    xmldoc.Save(writer);
                }

                // Result will indicate the document path.
                result.Result = output.OutputFilePath;
            }
        }
        else
        {
            // Signed XML document is returned from task.
            result.Result = xmldoc.OuterXml;
        }

        return result;
    }
}
