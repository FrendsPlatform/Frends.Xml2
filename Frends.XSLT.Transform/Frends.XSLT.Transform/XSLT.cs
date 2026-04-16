using Frends.XSLT.Transform.Definitions;
using Saxon.Api;
using System.ComponentModel;
using System.Xml;

namespace Frends.XSLT.Transform;

// We want to use all caps for abbreviations, but it is impossible to tell
// SonarLint about our exceptions (e.g. XSLT).
// Thus we disable this with pragma until SonarLint has an exception word list.

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
    /// <returns>Object { string XML }</returns>
    public static Result Transform([PropertyTab] Input input, [PropertyTab] Options options)
    {
        var processor = Saxon.Activation.Activator.Activate();
        var compiler = processor.NewXsltCompiler();

        using var stringReader = new StringReader(input.XSLT);
        var executable = compiler.Compile(stringReader);
        var transformer = executable.Load();

        using var inputStream = new MemoryStream();
        using var stringWriter = new StringWriter();

        try
        {
            XmlDocument xmldoc = new();
            xmldoc.PreserveWhitespace = true;

            var xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                XmlResolver = options.EnableExternalEntities ? new XmlUrlResolver() : null
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
                XML = output
            };
        }
        catch (Exception ex)
        {
            var wrapper = new Exception("Error running XSLT transformation", ex);
            throw wrapper;
        }
        finally
        {
            stringReader.Close();
            stringWriter.Close();
            inputStream.Close();
        }
    }
}