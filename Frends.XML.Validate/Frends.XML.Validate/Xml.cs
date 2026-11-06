using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Frends.Xml.Validate.Definitions;
using Frends.Xml.Validate.Helpers;

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
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, bool IsValid, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static Result Validate([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(input.Xml);

            var validateResult = new Result { Success = true, IsValid = true };
            var schemas = new XmlSchemaSet();

            var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            foreach (var schema in input.XsdSchemas)
            {
                cancellationToken.ThrowIfCancellationRequested();
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
                    validateResult.Error = new Error { Message = e.Message, AdditionalInfo = e.Exception };
                });

            return validateResult;
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
    }
}
