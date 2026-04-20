using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Frends.Xml.Validate.Definitions;

namespace Frends.Xml.Validate;

/// <summary>
/// Frends task for validating XML.
/// </summary>
public static class Xml
{
    /// <summary>
    /// Validates XML using XSD schemas. 
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.XML.Validate)
    /// </summary>
    /// <param name="input">Input definition.</param>
    /// <param name="options">Task options.</param>
    /// <returns>Object { bool IsValid, string Error }</returns>
    public static Result Validate([PropertyTab] Input input, [PropertyTab] Options options)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(input.Xml);

        var validateResult = new Result { IsValid = true };
        var schemas = new XmlSchemaSet();

        var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema};
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

        foreach (var schema in input.XsdSchemas)
        {
            schemas.Add(null, XmlReader.Create(new StringReader(schema), settings));
        }

        XDocument.Load(new XmlNodeReader(xmlDocument)).Validate(
            schemas,
            (o, e) =>
            {
                if (options.ThrowOnValidationErrors)
                {
                    throw new XmlSchemaValidationException(e.Message, e.Exception);
                }

                validateResult.IsValid = false;
                validateResult.Error = e.Message;
            });

        return validateResult;
    }
}