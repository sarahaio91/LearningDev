using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace vzr_common
{
    public static class XDocumentExtensions
    {
        public static void Write(this XDocument document, string outputPath, Formatting formatting = Formatting.None)
        {
            using (
                var writer =
                    new XmlTextWriter(outputPath, Encoding.UTF8))
            {
                writer.Formatting = formatting;
                document.Save(writer);
            }
        }

        public static XDocument XslTransfrom(this XContainer document, string xslPath)
        {
            var transformedDoc = new XDocument();
            using (XmlWriter writer = transformedDoc.CreateWriter())
            {
                var xmlResolver = new XmlUrlResolver();
                var transform = new XslCompiledTransform();
                transform.Load(xslPath, new XsltSettings {EnableDocumentFunction = true}, xmlResolver);
                //transform.Load(XmlReader.Create(new StringReader(xslPath)));
                transform.Transform(document.CreateReader(), writer);
            }

            return transformedDoc;
        }

        public static XElement StripNS(this XElement root)
        {
            var res = new XElement(
                root.Name.LocalName,
                root.Nodes().Select(n => n is XElement ? StripNS(n as XElement) : n));

            res.ReplaceAttributes(
                root.Attributes().Where(attr => (!attr.IsNamespaceDeclaration)));

            return res;
        }
    }
}