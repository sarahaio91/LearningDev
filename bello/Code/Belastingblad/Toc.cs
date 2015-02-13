using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Common;

namespace Belastingblad
{
    public class Toc
    {
        public void Generate(string outputPath)
        {
            int order = 1;
            var navMap = new XElement("navMap");
            var np = new XElement("navPoint", new XAttribute("id", Guid.NewGuid()), new XAttribute("playOrder", order++));
            var nl = new XElement("navLabel");
            var t = new XElement("text", new XText("Inhoud"));
            var c = new XElement("content", new XAttribute("src", "Text/Inhoud.html"));
            nl.Add(t);
            np.Add(nl, c);
            navMap.Add(np);

            var mainSection = new InputData(Path.Combine(outputPath, "Section0000.html"));
            var body = mainSection.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));
            var divs = body.Elements()
                .Where(
                    e =>
                        "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                        "hftekst".Equals(e.Attribute("class").Value));
            bool found = false;
            foreach (var div in divs)
            {
                var h2 = div.Descendants().FirstOrDefault(e => "h2".Equals(e.Name.LocalName));
                found = h2 != null;
                if (found)
                {
                    var navPoint = new XElement("navPoint", new XAttribute("id", Guid.NewGuid()),
                        new XAttribute("playOrder", order++));
                    var navLabel = new XElement("navLabel");
                    var text = new XElement("text");
                    var content = new XElement("content", new XAttribute("src", "Text/Section0000.html#"));
                    text.Value = h2.Value.Trim();
                    var h2a = h2.Descendants()
                        .FirstOrDefault(e => "a".Equals(e.Name.LocalName) && e.Attribute("href") != null);
                    content.Attribute("src").Value += h2a.Attribute("id").Value;
                    navLabel.Add(text);
                    navPoint.Add(navLabel, content);
                    navMap.Add(navPoint);
                }
                else
                {
                    var firstdiv = div.Elements().FirstOrDefault();
                    var instantienaam = firstdiv.Descendants()
                        .FirstOrDefault(
                            e =>
                                "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                                "instantienaam".Equals(e.Attribute("class").Value));
                    if (instantienaam != null)
                    {
                        var navPoint = new XElement("navPoint", new XAttribute("id", Guid.NewGuid()),
                            new XAttribute("playOrder", order++));
                        var navLabel = new XElement("navLabel");
                        var text = new XElement("text");
                        var content = new XElement("content", new XAttribute("src", "Text/Section0000.html#"));
                        //content.Attribute("src").Value += h2a.Attribute("id").Value;
                        var firstdiva = firstdiv.Descendants()
                            .FirstOrDefault(e => "a".Equals(e.Name.LocalName) && e.Attribute("id") != null);
                        text.Value = firstdiva.Value.Trim();
                        content.Attribute("src").Value += firstdiva.Attribute("id").Value;
                        navLabel.Add(text);
                        navPoint.Add(navLabel, content);
                        navMap.Elements().Last().Add(navPoint);
                    }
                }
            }

            var nap = new XElement("navPoint", new XAttribute("id", Guid.NewGuid()), new XAttribute("playOrder", order++));
            var nal = new XElement("navLabel");
            var ta = new XElement("text", new XText("Colofon"));
            var ca = new XElement("content", new XAttribute("src", "Text/Section0000.html#colofon"));
            nal.Add(ta);
            nap.Add(nal, ca);
            navMap.Add(nap);

            var nap1 = new XElement("navPoint", new XAttribute("id", "paginaregister"), new XAttribute("playOrder", order++));
            var nal1 = new XElement("navLabel");
            var ta1 = new XElement("text", new XText("Paginaregister"));
            var ca1 = new XElement("content", new XAttribute("src", "Text/Section0000.html#paginaregister"));
            nal1.Add(ta1);
            nap1.Add(nal1, ca1);
            navMap.Add(nap1);

            using (
                var writer =
                    new XmlTextWriter(Path.Combine(outputPath, "toc.ncx"), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                navMap.Save(writer);
            }
        }
    }
}