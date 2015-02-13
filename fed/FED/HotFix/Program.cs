using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace HotFix
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateInhoud(Path.Combine(args[0], "artikelen.html"), Path.Combine(args[0], "inhoud.html"), Path.Combine(args[1], "inhoud.html"));
            UpdateArtikelen(Path.Combine(args[0], "artikelen.html"), Path.Combine(args[1], "artikelen.html"));
        }

        static void UpdateInhoud(string detailFile, string inputFile, string outputFile)
        {
            var doc = new InputData(inputFile);
            FixInhoud(detailFile, doc);
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(outputFile, Encoding.UTF8))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                doc.Document.Save(xmlTextWriter);
            }
            CleanUp(outputFile);
        }

        static void UpdateArtikelen(string inputFile, string outputFile)
        {
            var doc = new XmlDocument();
            if (File.Exists(outputFile)) File.Delete(outputFile);
            using (var tr = new XmlTextReader(new StreamReader(inputFile)))
            {
                tr.XmlResolver = null;
                doc.Load(tr);
                if (doc.DocumentType != null)
                {
                    var docType = doc.DocumentType;
                    doc.RemoveChild(docType);
                }
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(outputFile, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    doc.Save(xmlTextWriter);
                }
            }
            using (Stream strm = Assembly.GetExecutingAssembly().GetManifestResourceStream("HotFix.hotfix.xslt"))
            using (XmlReader xsl = XmlReader.Create(strm))
            {
                var transform = new XslTransform();
                transform.Load(xsl);
                transform.Transform(outputFile, outputFile);
            }
            CleanUp(outputFile);
        }

        static void FixInhoud(string detailFile, InputData doc)
        {
            var details = new InputData(detailFile);
            var body = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("body")).FirstOrDefault();
            if(body != null)
            {
                var element = new XElement("div", new XAttribute("class", "hftekst"), 
                    new XElement("div", new XAttribute("class", "balk"),
                        new XElement("h2", new XAttribute("class", "kopgeghftekst"),
                            new XAttribute("id", "calibre_toc_1"), new XElement("span", 
                                new XAttribute("class", "hfteksttitel"), new XText("Inhoud")))),
                    new XElement("table", new XAttribute("class", "calibre1")));
                var kopgeghfteksts = details.Document.Descendants().Where(i => i.Name.LocalName.Equals("div") &&
                    i.Attribute("class") != null && i.Attribute("class").Value.Equals("kopgeghftekst"));
                var table = element.Descendants().Where(i => i.Name.LocalName.Equals("table")).FirstOrDefault();
                foreach(var kopgeghftekst in kopgeghfteksts)
                {
                    if(table != null)
                    {
                        var titel = kopgeghftekst.Descendants().Where(i => i.Name.LocalName.Equals("span") &&
                            i.Attribute("class") != null && i.Attribute("class").Value.Equals("titel") &&
                            i.Attribute("id") != null).FirstOrDefault();
                        if (titel != null)
                        {
                            var id = titel.Attribute("id").Value;
                            if (!string.IsNullOrEmpty(id))
                            {
                                table.Add(new XElement("tr", new XAttribute("class", "calibre2"),
                                    new XElement("td", new XAttribute("class", "calibre3"), new XAttribute("colspan", "2"),
                                        new XElement("a", new XAttribute("class", "tochftekst"),
                                            new XAttribute("href", string.Format("../Text/artikelen.html#{0}", id)),
                                            new XAttribute("id", string.Format("back_{0}", id)),
                                            new XText(titel.Value)))));
                            }
                        }
                    }
                }
                table.Add(new XElement("tr", new XAttribute("class", "calibre2"),
                    new XElement("td", new XAttribute("class", "calibre3"), new XAttribute("colspan", "2"),
                        new XElement("div", new XAttribute("class", "ixhftekst"),
                            new XElement("a", new XAttribute("class", "ixlink"),
                                new XAttribute("href", "../Text/artikelen.html#toclink_paginaregister"),
                                new XAttribute("id", "backlink_paginaregister"), new XText("Paginaregister"))))));
                body.ReplaceNodes(element);
            }
            
        }

        static void CleanUp(string file)
        {
            var content = File.ReadAllText(file);
            var modified = false;
            if (content.Contains(" xmlns=\"\""))
            {
                content = content.Replace(" xmlns=\"\"", "");
                modified = true;
            }
            if (content.Contains(" xmlns=\"http://www.w3.org/1999/xhtml\""))
            {
                content = content.Replace(" xmlns=\"http://www.w3.org/1999/xhtml\"", "");
                modified = true;

            }
            if (content.Contains(" clear=\"none\""))
            {
                content = content.Replace(" clear=\"none\"", "");
                modified = true;
            }
            if (content.Contains((char)8239))
            {
                content = content.Replace((char)8239, ' ');
                modified = true;
            }

            if (modified) File.WriteAllText(file, content, Encoding.UTF8);
        }
    }
}

