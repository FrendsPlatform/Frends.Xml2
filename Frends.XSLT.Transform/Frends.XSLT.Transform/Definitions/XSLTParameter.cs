namespace Frends.XSLT.Transform.Definitions;

// We want to use all caps for abbreviations, but it is impossible to tell
// SonarLint about our exceptions (e.g., XSLT).
// Thus, we disable this with pragma until SonarLint has an exception word list.

/// <summary>
/// XSLT parameter for providing values for `xsl:param` elements.
/// </summary>
public class XSLTParameter
{
    private readonly string name = string.Empty;
    private readonly string value = string.Empty;

    /// <summary>
    /// XSLT parameter name.
    /// </summary>
    /// <example>param1</example>
    public string? Name
    {
        get => name;
        init => name = value ?? string.Empty;
    }

    /// <summary>
    /// XSLT parameter value.
    /// </summary>
    /// <example>value1</example>
    public string? Value
    {
        get => value;
        init => this.value = value ?? string.Empty;
    }
}
