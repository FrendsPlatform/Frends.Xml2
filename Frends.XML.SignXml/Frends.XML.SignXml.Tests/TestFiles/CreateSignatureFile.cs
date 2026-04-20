namespace Frends.XML.SignXml.Tests.TestFiles;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Utility-class for generating .pfx certificate for unit tests.
/// </summary>
public static class CreateSignatureFile
{
    /// <summary>
    /// Generate signature-file.
    /// </summary>
    /// <param name="path">Path where the file will be created.</param>
    /// <param name="password">Password for the signature file.</param>
    /// <summary>
    /// Utility class for generating .pfx certificate for unit tests.
    /// </summary>
    public static void GenerateSignatureFile(string path, string password)
    {
        using (RSA rsa = RSA.Create())
        {
            var request = new CertificateRequest(
                "cn=localhost",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
            request.CertificateExtensions.Add(
                new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

            var certificate = request.CreateSelfSigned(
                DateTimeOffset.UtcNow.AddDays(-1),
                DateTimeOffset.UtcNow.AddYears(1));

            byte[] certData = certificate.Export(X509ContentType.Pfx, password);
            File.WriteAllBytes(path, certData);
        }
    }
}