using System.ComponentModel;
using System.Xml;
using Frends.XSLT.Transform.Definitions;
using Frends.XSLT.Transform.Helpers;
using Saxon.Api;

namespace Frends.XSLT.Transform;

/// <summary>
/// Frends task for transforming XML with XSLT.
/// </summary>
public static class XSLT
{
    /// <summary>
    /// Transforms XML using XSLT. The XSLT version (1.0, 2.0, or 3.0) is determined by the version attribute in the stylesheet element. The version attribute is required.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.XSLT.Transform)
    /// </summary>
    /// <param name="input">Input definition.</param>
    /// <param name="options">Optional parameters.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, string XML, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static Result Transform([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        using var inputStream = new MemoryStream();
        using var stringWriter = new StringWriter();
        using var stringReader = new StringReader(input.XSLT);

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var processor = Saxon.Activation.Activator.Activate();
            var compiler = processor.NewXsltCompiler();

            var executable = compiler.Compile(stringReader);
            var transformer = executable.Load();

            XmlDocument xmldoc = new();
            xmldoc.PreserveWhitespace = true;

            var xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                XmlResolver = options.EnableExternalEntities ? new XmlUrlResolver() : null,
            };

            using (var xmlReader = XmlReader.Create(new StringReader(input.XML), xmlReaderSettings))
            {
                xmldoc.Load(xmlReader);
            }

            xmldoc.Save(inputStream);
            inputStream.Position = 0;

            var builder = processor.NewDocumentBuilder();
            transformer.XmlDocumentResolver = new ResourceResolver((request) =>
            {
                try
                {
                    Uri targetUri = request.Uri;
                    if (targetUri != null && targetUri.IsFile && File.Exists(targetUri.LocalPath))
                    {
                        byte[] fileData = File.ReadAllBytes(targetUri.LocalPath);

                        var ms = new MemoryStream(fileData);
                        var settings = new XmlReaderSettings { CloseInput = true };
                        var reader = XmlReader.Create(ms, settings, targetUri.ToString());

                        return new XmlReaderResource(builder, reader);
                    }
                }
                catch
                {
                    // Fallback to the underlying resolver if the URI is not a local file or an error occurs.
                }

                return null;
            });

            transformer.SetInputStream(inputStream, new Uri("file:///C:/example.txt"));
            input.XSLTParameters?.ToList().ForEach(x =>
                transformer.SetParameter(new QName(x.Name), new XdmAtomicValue(x.Value)));

            var serializer = processor.NewSerializer();
            serializer.OutputWriter = stringWriter;
            transformer.Run(serializer);
            var output = stringWriter.GetStringBuilder().ToString();

            // By default Saxon always produces \n
            output = output.Replace("\n", Environment.NewLine);
            return new Result
            {
                Success = true,
                XML = output,
                Error = null,
            };
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
        finally
        {
            stringReader.Close();
            stringWriter.Close();
            inputStream.Close();
        }
    }
}
