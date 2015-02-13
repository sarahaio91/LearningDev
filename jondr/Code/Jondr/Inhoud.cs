using System.IO;
using System.Linq;
using System.Xml.Linq;
using Common;
using vzr_common;
using System.Xml;
using System.Text;

namespace JournalBUSINESSgsrecht
{
    public class Inhoud
    {
        private const string Template =
            @"<?xml version='1.0' encoding='utf-8'?><html xmlns=""http://www.w3.org/1999/xhtml""><head><title>Journaal Ondernemingsrecht Journaal Ondernemingsrecht 1 1</title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/><link href=""../Styles/stylesheet.css"" rel=""stylesheet"" type=""text/css""/><link href=""../Styles/page_styles.css"" rel=""stylesheet"" type=""text/css""/></head><body class=""calibre""><h3 class=""index"" id=""inhoudsopgave"">Inhoud</h3><div class=""inhoudsopgave""><table class=""calibre1""></table></div><div class=""inhoudsopgave""><h3 class=""index"" id=""calibre_toc_2"">Colofon</h3><img src=""../Images/colofon.jpg"" alt="""" class=""calibre3""/></div></body></html>";
       
        public void Generate(string outputPath, string title)
        {
            var jrvartikelen = new InputData(Path.Combine(outputPath, "jrvartikelen.xhtml"));
            var jeartikelen = new InputData(Path.Combine(outputPath, "jeartikelen.xhtml"));

            var tmp = XDocument.Parse(Template);
            var tmpTable = tmp.Descendants().First(e => "table".Equals(e.Name.LocalName));

            var jrvchaps = jrvartikelen.Document.Descendants()
                .Where(
                    e =>
                        "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                        "hftekst".Equals(e.Attribute("class").Value));
            var jechaps = jeartikelen.Document.Descendants()
                .Where(
                    e =>
                        "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                        "hftekst".Equals(e.Attribute("class").Value));

            var jrvtr = new XElement("tr", new XAttribute("class", "calibre2"), new XElement("td", new XAttribute("class", "ixhftekst"), new XElement("span", new XAttribute("class", "titel"), new XText("JRV "))));
            var jetr = new XElement("tr", new XAttribute("class", "calibre2"), new XElement("td", new XAttribute("class", "ixhftekst"), new XElement("span", new XAttribute("class", "titel"), new XText("JE "))));

            tmpTable.Add(jrvtr);
            foreach (var jrvchap in jrvchaps)
            {
                var tr = new XElement("tr", new XAttribute("class", "calibre2"));
                var td = new XElement("td", new XAttribute("class", "ixhftekst"));
                var a = new XElement("a", new XAttribute("href", "../Text/jrvartikelen.xhtml#"), new XAttribute("id", "back_"), new XAttribute("class", "ixlink"));
                var span = new XElement("span", new XAttribute("class", "titel"));
                var ajrvchap = jrvchap.Descendants().First(e => "a".Equals(e.Name.LocalName));
                a.Attribute("href").Value += ajrvchap.Attribute("id").Value;
                a.Attribute("id").Value += ajrvchap.Attribute("id").Value;
                span.Value = ajrvchap.Elements().First(e => "span".Equals(e.Name.LocalName)).Value;
                
                a.Add(span);
                td.Add(a);
                tr.Add(td);
                tmpTable.Add(tr);
            }

            tmpTable.Add(jetr);
            foreach (var jechap in jechaps)
            {
                var tr = new XElement("tr", new XAttribute("class", "calibre2"));
                var td = new XElement("td", new XAttribute("class", "ixhftekst"));
                var a = new XElement("a", new XAttribute("href", "../Text/jeartikelen.xhtml#"), new XAttribute("id", "back_"), new XAttribute("class", "ixlink"));
                var span = new XElement("span", new XAttribute("class", "titel"));
                var ajrvchap = jechap.Descendants().First(e => "a".Equals(e.Name.LocalName));
                a.Attribute("href").Value += ajrvchap.Attribute("id").Value;
                a.Attribute("id").Value += ajrvchap.Attribute("id").Value;
                span.Value = ajrvchap.Elements().First(e => "span".Equals(e.Name.LocalName)).Value;

                a.Add(span);
                td.Add(a);
                tr.Add(td);
                tmpTable.Add(tr);
            }

            tmp.Write(Path.Combine(outputPath, "inhoud.xhtml"));
            FixDuplicatedId(Path.Combine(outputPath, "inhoud.xhtml"), outputPath);
        }

        public void FixDuplicatedId(string file, string outputPath)
        {
            var doc = new InputData(file);
            var links = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("a") &&
                i.Attribute("class") != null && i.Attribute("class").Value.Equals("ixlink") &&
                i.Attribute("id") != null);
            var count = 0;
            foreach(var link in links)
            {
                var id = link.Attribute("id").Value;
                var duplicatedLinks = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("a") && 
                    i.Attribute("id") != null && i.Attribute("id").Value.Equals(id));
                if(duplicatedLinks != null && duplicatedLinks.Count() > 1)
                {
                    count++;
                    for(var j = 1; j < duplicatedLinks.Count(); j++)
                    {
                        var oldId = duplicatedLinks.ElementAt(j).Attribute("id").Value;
                        var newId = string.Format("back_ixlink{0}", count);
                        var fileName = duplicatedLinks.ElementAt(j).Attribute("href").Value.
                            Replace("../Text/", "").Replace("#" + oldId.Replace("back_", ""), "");
                        duplicatedLinks.ElementAt(j).SetAttributeValue("href", string.Format("{0}#{1}", 
                            duplicatedLinks.ElementAt(j).Attribute("href").Value.Replace("#" + oldId.Replace("back_", ""), ""),
                            newId.Replace("back_", "")));
                        duplicatedLinks.ElementAt(j).SetAttributeValue("id", newId);
                        var artikelen = new InputData(Path.Combine(outputPath, fileName));
                        var detailLink = artikelen.Document.Descendants().Where(i => i.Name.LocalName.Equals("a") &&
                            i.Attribute("id") != null && i.Attribute("id").Value.Equals(oldId.Replace("back_", ""))).FirstOrDefault();
                        if(detailLink != null)
                        {
                            detailLink.SetAttributeValue("id", newId.Replace("back_", ""));
                            detailLink.SetAttributeValue("href", detailLink.Attribute("href").Value.
                                Replace("#" + oldId, "#" + newId));
                        }

                        using (XmlTextWriter xmlTextWriter = new XmlTextWriter(Path.Combine(outputPath, 
                            fileName.Replace(".xhtml", "") + "_new.xhtml"), Encoding.UTF8))
                        {
                            xmlTextWriter.Formatting = Formatting.Indented;
                            artikelen.Document.Save(xmlTextWriter);
                        }
                         
                    }
                }
            }
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(Path.Combine(outputPath, 
                Path.GetFileNameWithoutExtension(doc.FileName) + "_new.xhtml"), Encoding.UTF8))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                doc.Document.Save(xmlTextWriter);
            }
        }
    }
}