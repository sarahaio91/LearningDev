using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Common;

namespace JournalBUSINESSgsrecht
{
    public class Toc
    {
        public void Generate(string outputPath)
        {
            Clean(outputPath);
            var jrvartikelen = new InputData(Path.Combine(outputPath, "jrvartikelen.xhtml"));
            var jeartikelen = new InputData(Path.Combine(outputPath, "jeartikelen.xhtml"));
            var navMap = new XElement("navMap");
            int playOrder = 1;

            var inhoudNavPoint = new XElement("navPoint", 
                new XAttribute("id", Guid.NewGuid().ToString().ToLower()),
                new XAttribute("playOrder", playOrder++),
                new XElement("navLabel", new XElement("text", new XText("Inhoud"))),
                new XElement("content", new XAttribute("src", "Text/inhoud.xhtml#inhoudsopgave")));
            navMap.Add(inhoudNavPoint);

            var colofonNavPoint = new XElement("navPoint",
                new XAttribute("id", Guid.NewGuid().ToString().ToLower()),
                new XAttribute("playOrder", playOrder++),
                new XElement("navLabel", new XElement("text", new XText("Colofon"))),
                new XElement("content", new XAttribute("src", "Text/inhoud.xhtml#calibre_toc_2")));
            navMap.Add(colofonNavPoint);

            XElement jrvbody = jrvartikelen.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));
            XElement jebody = jeartikelen.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));

            var jrvNavPoint = new XElement("navPoint",
                new XAttribute("id", Guid.NewGuid().ToString().ToLower()),
                new XAttribute("playOrder", playOrder++),
                new XElement("navLabel", new XElement("text", new XText("JRV"))),
                new XElement("content", new XAttribute("src", "Text/jrvartikelen.xhtml")));
            navMap.Add(jrvNavPoint);
          
            foreach (XElement element in jrvbody.Elements())
            {
                var navPoint = new XElement("navPoint", new XAttribute("id", Guid.NewGuid().ToString().ToLower()),
                    new XAttribute("playOrder", playOrder++));
                var navLabel = new XElement("navLabel");
                var text = new XElement("text");
                var content = new XElement("content", new XAttribute("src", "Text/jrvartikelen.xhtml#"));

                if ("hftekst".Equals(element.Attribute("class").Value))
                {
                    var ajrvchap = element.Descendants().First(e => "a".Equals(e.Name.LocalName));
                    text.Value = ajrvchap.Value;
                    content.Attribute("src").Value += ajrvchap.Attribute("id").Value;
                   
                    navLabel.Add(text);
                    navPoint.Add(navLabel, content);
                    jrvNavPoint.Add(navPoint);
                }
                else
                {
                    var vindplaats = element.Descendants()
                        .First(
                            e =>
                                "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                                "vindplaats".Equals(e.Attribute("class").Value));
                    text.Value = vindplaats.Value;
                    content.Attribute("src").Value += vindplaats.Attribute("id").Value;
                    
                    navLabel.Add(text);
                    navPoint.Add(navLabel, content);
                    jrvNavPoint.Elements().Last().Add(navPoint);
                }
                //navLabel.Add(text);
                //navPoint.Add(navLabel, content);
                //navMap.Add(navPoint);
            }

            var jeNavPoint = new XElement("navPoint",
                new XAttribute("id", Guid.NewGuid().ToString().ToLower()),
                new XAttribute("playOrder", playOrder++),
                new XElement("navLabel", new XElement("text", new XText("JE"))),
                new XElement("content", new XAttribute("src", "Text/jeartikelen.xhtml")));
            navMap.Add(jeNavPoint);

            foreach (XElement element in jebody.Elements())
            {
                var navPoint = new XElement("navPoint", new XAttribute("id", Guid.NewGuid().ToString().ToLower()),
                    new XAttribute("playOrder", playOrder++));
                var navLabel = new XElement("navLabel");
                var text = new XElement("text");
                var content = new XElement("content", new XAttribute("src", "Text/jeartikelen.xhtml#"));

                if ("hftekst".Equals(element.Attribute("class").Value))
                {
                    var ajrvchap = element.Descendants().First(e => "a".Equals(e.Name.LocalName));
                    text.Value = ajrvchap.Value;
                    content.Attribute("src").Value += ajrvchap.Attribute("id").Value;

                    navLabel.Add(text);
                    navPoint.Add(navLabel, content);
                    jeNavPoint.Add(navPoint);

                }
                else
                {
                    var vindplaats = element.Descendants()
                        .First(
                            e =>
                                "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                                "vindplaats".Equals(e.Attribute("class").Value));
                    text.Value = vindplaats.Value;
                    content.Attribute("src").Value += vindplaats.Attribute("id").Value;

                    navLabel.Add(text);
                    navPoint.Add(navLabel, content);
                    jeNavPoint.Elements().Last().Add(navPoint);
                }
                //navLabel.Add(text);
                //navPoint.Add(navLabel, content);
                //navMap.Add(navPoint);
            }

            using (
                var writer =
                    new XmlTextWriter(Path.Combine(outputPath, "toc.ncx"), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                navMap.Save(writer);
            }

            
        }

        public void Clean(string outputPath)
        {
            foreach(var file in Directory.GetFiles(outputPath, "*.*").Where(i => Path.GetFileName(i).Contains("_new")))
            {
                var relevantFile = Path.GetFileNameWithoutExtension(file).Replace("_new", "") + ".xhtml";
                File.Delete(Path.Combine(outputPath, relevantFile));
                File.Move(file, Path.Combine(outputPath, relevantFile));
            }
        }
    }
}