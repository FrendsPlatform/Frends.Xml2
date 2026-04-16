using System.ComponentModel;

namespace Frends.XML.XPathQuery.Definitions;

/// <summary>
/// Query task options.
/// </summary>
public class Options
{
    /// <summary>
    /// Throw an exception if no results returned by query
    /// </summary>
    /// <example>true</example>
    [DefaultValue("true")]
    public bool ThrowErrorOnEmptyResults { get; set; } = true;

    /// <summary>
    /// XPath Query language version. Version 1.0 is not supported.
    /// V31 = 3.1
    /// V30 = 3.0
    /// V20 = 2.0
    /// </summary>
    /// <example>XPathVersion.V31</example>
    [DefaultValue(XPathVersion.V31)]
    public XPathVersion XPathVersion { get; set; }

    /// <summary>
    /// Enables the Task to return the raw xml values for non atomic values.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool ReturnRawXmlForNonAtomicValues { get; set; }

    /// <summary>
    /// Enable schema aware processing.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool SchemaAware { get; set; } = false;

    /// <summary>
    /// Schema validation mode.
    /// </summary>
    /// <example>SchemaValidationMode.None</example>
    [DefaultValue(SchemaValidationMode.None)]
    public SchemaValidationMode SchemaValidationMode { get; set; } = SchemaValidationMode.None;

    /// <summary>
    /// Controls whether external entities are allowed in the XML processing.
    /// </summary>
    /// <example>false</example>
    public bool EnableExternalEntities { get; set; }
}