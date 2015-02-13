using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Common;
using GeneratePageRef;
using vzr_common;

namespace FED
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string outputPath = @"C:\Epub\fed\output";
            string hfccPath = @"C:\Epub\fed\input\hf-cc.xml";
            new Content().Generate(hfccPath, outputPath, "FED Fiscaal Weekblad 2014 - 22", "");
            new Inhoud().Generate(hfccPath, outputPath, "FED Fiscaal Weekblad 2014 - 22");
            new Toc().Generate(hfccPath, outputPath, "FED Fiscaal Weekblad 2014 - 22");
            GeneratePageRef(outputPath, 3, 24);
            CleanUp(Path.Combine(outputPath, "artikelen.html"));
            CleanUp(Path.Combine(outputPath, "inhoud.html"));
            CleanUp(Path.Combine(outputPath, "toc.xml"));
            CleanUp(Path.Combine(outputPath, "pageref.xml"));

        }

        private static void GeneratePageRef(string outputPath, int start, int end)
        {
            var body = new XElement("body");
            var tablepageref = new XElement("div", new XAttribute("class", "table"));
            var linkpageref = new XElement("div", new XAttribute("class", "details"));
            for (int i = start; i <= end; i++)
            {
                var spanpagereglinkdiv = new XElement("span", new XAttribute("class", "pagereglinkdiv"));
                var apagereglinkdiv = new XElement("a", new XAttribute("class", "pagereglink"),
                    new XAttribute("href", "#pageref_" + i), new XAttribute("id", "backpageref_" + i),
                    new XText(i.ToString()));
                spanpagereglinkdiv.Add(apagereglinkdiv);
                tablepageref.Add(spanpagereglinkdiv);

                var pagerefdiv = new XElement("div", new XAttribute("class", "pagerefdiv"));
                var a = new XElement("a", new XAttribute("class", "pcalibre pageref pcalibre1"),
                    new XAttribute("href", "#backpageref_" + i),
                    new XAttribute("id", "pageref_" + i),
                    new XText(i.ToString()));
                pagerefdiv.Add(a);
                linkpageref.Add(pagerefdiv);
            }
            body.Add(tablepageref, linkpageref);
            using (
                var writer =
                    new XmlTextWriter(Path.Combine(outputPath, "pageref.xml"), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                body.Save(writer);
            }
        }

        private static void CleanUp(string file)
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
            if (modified) File.WriteAllText(file, content.Replace((char)8239, ' '), Encoding.UTF8);
        }

    }

    public class Content
    {
        public void Generate(string hfccPath, string outputPath, string title, string pdfPath, string footerPrefix = "FED 2014/")
        {
            //string newHfcc = Path.Combine(Path.GetDirectoryName(outputPath), "fixPageRef.xml");
            //using (var writer = new XmlTextWriter(newHfcc, Encoding.UTF8))
            //{
            //    new PdfManipulating().FixPageRef(hfccPath, pdfPath, footerPrefix).Save(writer);
            //}

            XDocument hfcc = XDocument.Load(hfccPath);
            XElement hf = hfcc.Elements().First(e => "hf".Equals(e.Name.LocalName));
            hf.Add(new XAttribute("title", title));
            XDocument document = hfcc.XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\artikelen.xslt"));
            //AddPageRegLink(document);
            document
                .Write(Path.Combine(outputPath, "artikelen.html"), Formatting.Indented);
            FixList(Path.Combine(outputPath, "artikelen.html"));
        }

        private void AddPageRegLink(XDocument doc)
        {
            var paginaregister = new XElement("div",
                           new XAttribute("class", "ablok"),
                           new XElement("a",
                               new XAttribute("href", "inhoud.html#backlink_paginaregister"),
                               new XAttribute("id", "toclink_paginaregister"),
                               new XAttribute("class", "vindplaats1"),
                               new XText("Paginaregister")));
            var pagerefdivs = doc.Descendants().Where(e => "span".Equals(e.Name.LocalName) && e.Attributes("class").Any(a => "pagerefdiv".Equals(a.Value)));
            for (var i = 0; i < pagerefdivs.Count(); i++)
            {
                var a = pagerefdivs.ElementAt(i).Elements().First();
                var idx = string.Empty;
                if (a != null && !string.IsNullOrEmpty(a.Value))
                    idx = a.Value;

                paginaregister.Add(new XElement("span",
                    new XAttribute("class", "pagereglinkdiv"),
                    new XElement("a",
                        new XAttribute("class", "pagereglink"),
                        new XAttribute("href", "../Text/artikelen.html#pageref_" + idx),
                        new XAttribute("id", "pageindex_" + idx),
                        new XText(idx)
                    )));
            }
            doc.Root.Elements().First(e => "body".Equals(e.Name.LocalName)).Add(paginaregister);
        }

        void FixList(string file)
        {
            var modified = false;
            var doc = new InputData(file);
            var spans = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("span") && i.Attribute("class") != null &&
                i.Attribute("class").Value.Equals("prefixAblok"));
            foreach (var span in spans)
            {
                var parent = span.Parent;
                if (parent != null && parent is XElement)
                {
                    var parentElement = parent as XElement;
                    if (parentElement.Name.LocalName.Equals("div") && parentElement.Attribute("class") != null &&
                        parentElement.Attribute("class").Value.Equals("kopgegp2"))
                    {
                        var nextNode = parentElement.NextNode.NextNode;
                        if (nextNode != null && nextNode is XElement)
                        {
                            var nextElement = nextNode as XElement;
                            if (nextElement.Name.LocalName.Equals("span") && nextElement.Attribute("class") != null &&
                                    nextElement.Attribute("class").Value.Equals("ablok"))
                            {
                                nextElement.AddFirst(new XText(span.Value));
                                modified = true;
                            }
                        }
                    }
                }
            }
            spans.Remove();
            if (modified)
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(file, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    doc.Document.Save(xmlTextWriter);
                }
            }

            CleanUp(file);
        }

        void CleanUp(string file)
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

    public class Inhoud
    {
        public void Generate(string hfccPath, string outputPath, string title)
        {
            var artikelen = new InputData(Path.Combine(outputPath, "artikelen.html"));
            XElement body = artikelen.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));
            body.Add(new XAttribute("title", title));
            XDocument document = body.StripNS().XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\inhoud.xslt"));
            document
                .Write(Path.Combine(outputPath, "inhoud.html"), Formatting.Indented);
        }
    }

    public class Toc
    {
        public void Generate(string hfccPath, string outputPath, string title)
        {
            XDocument hfcc = XDocument.Load(hfccPath);
            XElement hf = hfcc.Elements().First(e => "hf".Equals(e.Name.LocalName));
            hf.Add(new XAttribute("title", title));
            XDocument document = hfcc.XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\toc.xslt"));
            IEnumerable<XElement> navPoints = document.Descendants().Where(e => "navPoint".Equals(e.Name.LocalName));
            int playOrder = 1;
            foreach (var navPoint in navPoints)
            {
                navPoint.Add(new XAttribute("id", Guid.NewGuid().ToString().ToLower()));
                navPoint.Add(new XAttribute("playOrder", playOrder++));
            }
            document
                .Write(Path.Combine(outputPath, "toc.xml"), Formatting.Indented);
        }
    }

}
