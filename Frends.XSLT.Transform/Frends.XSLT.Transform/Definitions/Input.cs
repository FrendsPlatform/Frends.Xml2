namespace Frends.XSLT.Transform.Definitions;

/// <summary>
/// Transform task input.
/// </summary>
public class Input
{
    /// <summary>
    /// XML to transform.
    /// </summary>
    /// <example><xml>...</xml></example>
    public string XML { get; set; } = "";

    /// <summary>
    /// XSLT transformation. The XSLT version (1.0, 2.0, or 3.0) is determined by the version attribute in the stylesheet element. The version attribute is required.
    /// </summary>
    /// <example>
    /// &lt;xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0"&gt;
    ///   &lt;xsl:template match="/"&gt;
    ///     &lt;result&gt;&lt;xsl:value-of select="//item"/&gt;&lt;/result&gt;
    ///   &lt;/xsl:template&gt;
    /// &lt;/xsl:stylesheet&gt;
    /// </example>
    public string XSLT { get; set; } = "";

    /// <summary>
    /// XSLT parameters.
    /// </summary>
    /// <example>[
    /// { Name = "param", Value = "foo" },
    /// { Name = "param2", Value = "bar" }
    /// ]</example>
    public XSLTParameter[] XSLTParameters { get; set; } = Array.Empty<XSLTParameter>();
}