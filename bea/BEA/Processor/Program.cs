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

namespace KWEP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string outputPath = @"C:\Epub\bea\output";
            string hfccPath = @"C:\Epub\bea\input\hf-cc.xml";
            new Content().Generate(hfccPath, outputPath, "Belastingadvies 2014-22", "");
            new Content().GeneratePageRef(outputPath, 3, 27);
            new Inhoud().Generate(Path.Combine(outputPath, "artikelen.html"), outputPath, "Belastingadvies 2014-22");
            new Toc().Generate(Path.Combine(outputPath, "artikelen.html"), outputPath, "Belastingadvies 2014-22");

            foreach(var file in Directory.GetFiles(outputPath, "*.*").Where(i => 
                Path.GetFileName(i).EndsWith(".html") || Path.GetFileName(i).EndsWith(".xml")))
            {
                CleanUp(file);
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
        public void Generate(string hfccPath, string outputPath, string title, string pdfPath)
        {
            //string newHfcc = Path.Combine(Path.GetDirectoryName(outputPath), "fixPageRef.xml");
            //using (var writer = new XmlTextWriter(newHfcc, Encoding.UTF8))
            //{
            //    new PdfManipulating().FixPageRef(hfccPath, pdfPath, footer).Save(writer);
            //}

            XDocument hfcc = XDocument.Load(hfccPath);
            XElement hf = hfcc.Elements().First(e => "hf".Equals(e.Name.LocalName));
            hf.Add(new XAttribute("title", title));
            XDocument document = hfcc.XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\artikelen.xslt"));
            AddPageRegLink(document);
            document
                .Write(Path.Combine(outputPath, "artikelen.html"), Formatting.Indented);
        }

        public void GeneratePageRef(string outputPath, int start, int end)
        {
            var body = new XElement("body");
            var footer = new XElement("div", new XAttribute("class", "footer"));
            var details = new XElement("div", new XAttribute("class", "details"));
            for (int i = start; i <= end; i++)
            {
                var spanpagereglinkdiv = new XElement("span", new XAttribute("class", "pagereglinkdiv"));
                var apagereglinkdiv = new XElement("a", new XAttribute("class", "pagereglink"),
                    new XAttribute("href", "#pageref_" + i), new XAttribute("id", "pageindex_" + i),
                    new XText(i.ToString()));
                spanpagereglinkdiv.Add(apagereglinkdiv);
                footer.Add(spanpagereglinkdiv);

                var pagerefdiv = new XElement("span", new XAttribute("class", "pagerefdiv"));
                var a = new XElement("a", new XAttribute("class", "pcalibre pageref pcalibre1"),
                    new XAttribute("href", "#pageindex_" + i),
                    new XAttribute("id", "pageref_" + i),
                    new XText(i.ToString()));
                pagerefdiv.Add(a);
                details.Add(pagerefdiv);
            }
            body.Add(footer, details);
            using (
                var writer =
                    new XmlTextWriter(Path.Combine(outputPath, "pageref.xml"), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                body.Save(writer);
            }
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
    }

    public class Inhoud
    {
        public void Generate(string inputFile, string outputPath, string title)
        {
            XDocument inhoud = XDocument.Load(inputFile);
            XElement body = inhoud.Descendants().Where(i => i.Name.LocalName.Equals("body")).FirstOrDefault();
            body.Add(new XAttribute("title", title));
            XDocument document = inhoud.XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\inhoud.xslt"));
            document
                .Write(Path.Combine(outputPath, "inhoud.html"), Formatting.Indented);
            
            //var artikelen = new InputData(Path.Combine(outputPath, "artikelen.html"));
            //XElement body = artikelen.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));
            //var inhoud = new XElement("html", new XElement("head", new XElement("title", new XText(title)),
            //    new XElement("link", new XAttribute("href", "../Styles/stylesheet.css"), 
            //        new XAttribute("type", "text / css"), new XAttribute("rel", "stylesheet"))),
            //    new XElement("body", new XAttribute("class", "body1"), 
            //        new XElement("div", new XAttribute("class", "inhoudsopgave"),
            //            new XElement("div", new XAttribute("class", "kopgeghftekst1"), 
            //                new XAttribute("id", "inhoudsopgave"), new XText("Inhoudsopgave")))));
            //var inhoudsopgave = inhoud.Descendants().Where(i => i.Name.LocalName.Equals("div") &&
            //    i.Attribute("class") != null && i.Attribute("class").Value.Equals("inhoudsopgave")).FirstOrDefault();
            //var kopgeghfteksts = body.Descendants().Where(i => i.Name.LocalName.Equals("p") &&
            //    i.Attribute("class") != null && i.Attribute("class").Value.Equals("kopgeghftekst") &&
            //    i.Attribute("id") != null);
            //foreach(var kopgeghftekst in kopgeghfteksts)
            //{
            //    var toc1 = new XElement("p", new XAttribute("class", "toc1"),
            //        new XElement("a", new XAttribute("href", string.Format("../Text/artikelen.html#{0}", kopgeghftekst.Attribute("id").Value)),
            //            new XAttribute("id", string.Format("back{0}", kopgeghftekst.Attribute("id").Value)),
            //            new XText(kopgeghftekst.Value)));
            //    inhoudsopgave.Add(toc1);
            //}
            //var ch_titles = body.Descendants().Where(i => i.Name.LocalName.Equals("p") &&
            //        i.Attribute("class") != null && (i.Attribute("class").Value.Equals("ch-title") ||
            //        i.Attribute("class").Value.Equals("ch-title1")));
            //foreach (var ch_title in ch_titles)
            //{
            //    var preNode = ch_title.PreviousNode;
            //    while(preNode != null && !(preNode is XElement))
            //    {
            //        preNode = preNode.PreviousNode;
            //    }
            //    if (preNode is XElement)
            //    {
            //        var preElement = preNode as XElement;
            //        if (preElement.Name.LocalName.Equals("p") && preElement.Attribute("class") != null &&
            //            (ch_title.Attribute("class").Value.Equals("ch-title") ? preElement.Attribute("class").Value.Equals("ch-num") :
            //            preElement.Attribute("class").Value.Equals("ch-num1")) && preElement.Attribute("id") != null)
            //        {
            //            var toc2 = new XElement("p", new XAttribute("class", "toc2"),
            //                new XElement("a", new XAttribute("href", string.Format("../Text/artikelen.html#{0}", preElement.Attribute("id").Value)),
            //                    new XAttribute("id", string.Format("back{0}", preElement.Attribute("id").Value)),
            //                    new XText(string.Format("{0} {1}", preElement.Value, ch_title.Value))));

            //            inhoudsopgave.Add(toc2);
            //        }
            //    }
            //}
            //inhoud.Save(Path.Combine(outputPath, "inhoud.html"));
        }
    }

    public class Toc
    {
        public void Generate(string inputFile, string outputPath, string title)
        {
            XDocument ncx = XDocument.Load(inputFile);
            XElement body = ncx.Descendants().Where(i => i.Name.LocalName.Equals("body")).FirstOrDefault();
            body.Add(new XAttribute("title", title));
            XDocument document = ncx.XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\toc.xslt"));
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
