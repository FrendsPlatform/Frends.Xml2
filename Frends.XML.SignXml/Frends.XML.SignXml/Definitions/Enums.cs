namespace Frends.XML.SignXml.Definitions;

using System;

/// <summary>
/// Can be either a file or an XML string.
/// </summary>
public enum XmlParamType
{
    /// <summary>
    /// Represents a file.
    /// </summary>
    File,

    /// <summary>
    /// Represents an XML string.
    /// </summary>
    XmlString,
}

/// <summary>
/// XML signature strategy for SignXml-task.
/// </summary>
public enum SigningStrategyType
{
    /// <summary>
    /// Uses a private key certificate for signing.
    /// </summary>
    PrivateKeyCertificate,
}

/// <summary>
/// Signature enveloping type for SignXml-task.
/// </summary>
public enum XmlEnvelopingType
{
    /// <summary>
    /// Uses an enveloped XML signature.
    /// </summary>
    XmlEnvelopedSignature,
}

/// <summary>
/// Signature methods for XMLDSIG.
/// </summary>
public enum XmlSignatureMethod
{
    /// <summary>
    /// RSA with SHA-1.
    /// </summary>
    RSASHA1,

    /// <summary>
    /// RSA with SHA-256.
    /// </summary>
    RSASHA256,

    /// <summary>
    /// RSA with SHA-384.
    /// </summary>
    RSASHA384,

    /// <summary>
    /// RSA with SHA-512.
    /// </summary>
    RSASHA512,
}

/// <summary>
/// Transform methods.
/// </summary>
public enum TransformMethod
{
    /// <summary>
    /// Canonical XML (C14N).
    /// </summary>
    DsigC14,

    /// <summary>
    /// Canonical XML with comments.
    /// </summary>
    DsigC14WithComments,

    /// <summary>
    /// Exclusive Canonical XML (Exc-C14N).
    /// </summary>
    DsigExcC14,

    /// <summary>
    /// Exclusive Canonical XML with comments.
    /// </summary>
    DsigExcC14WithComments,

    /// <summary>
    /// Base64 encoding.
    /// </summary>
    DsigBase64,
}

/// <summary>
/// Digest methods.
/// </summary>
public enum DigestMethod
{
    /// <summary>
    /// SHA-1 hashing algorithm.
    /// </summary>
    SHA1,

    /// <summary>
    /// SHA-256 hashing algorithm.
    /// </summary>
    SHA256,

    /// <summary>
    /// SHA-384 hashing algorithm.
    /// </summary>
    SHA384,

    /// <summary>
    /// SHA-512 hashing algorithm.
    /// </summary>
    SHA512,
}