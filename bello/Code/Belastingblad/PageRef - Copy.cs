using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Common;
using PdfUtility;
using vzr_common;

namespace Belastingblad
{
    public class PageRef
    {
        public void FixPageRef(string outputPath, string pdfFilePath, string footerPrefix = "Belastingblad 2014/")
        {
            var mainSection = new InputData(Path.Combine(outputPath, "Section0000.html"));
            var body = mainSection.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));
            var divs = body.Elements()
                .Where(
                    e =>
                        "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                        "hftekst".Equals(e.Attribute("class").Value));
            Dictionary<int, string> pdfPages = TextExtractor.Extract(pdfFilePath);
            
            var collected = new HashSet<int>();
            foreach (var div in divs)
            {
                foreach (var level1 in div.Elements())
                {
                    if (level1.Elements().Any(e => "div".Equals(e.Name.LocalName) && e.Attribute("class") != null && ("kopgegp".Equals(e.Attribute("class").Value) || "hftekst".Equals(e.Attribute("class").Value) || "auteurgeg".Equals(e.Attribute("class").Value))))
                    {
                        foreach (var level2 in level1.Elements())
                        {
                            if (level2.Elements().Any(e => "div".Equals(e.Name.LocalName) && e.Attribute("class") != null && ("kopgegp".Equals(e.Attribute("class").Value) || "hftekst".Equals(e.Attribute("class").Value) || "auteurgeg".Equals(e.Attribute("class").Value))))
                            {
                                foreach (var level3 in level2.Elements())
                                {
                                    if (level3.Elements().Any(e => "div".Equals(e.Name.LocalName) && e.Attribute("class") != null && ("kopgegp".Equals(e.Attribute("class").Value) || "hftekst".Equals(e.Attribute("class").Value) || "auteurgeg".Equals(e.Attribute("class").Value))))
                                    {
                                        foreach (var level4 in level3.Elements())
                                        {
                                            string val = ValueOfElement(level4);
                                            foreach (
                                                var pdfPage in pdfPages.Where(e => !collected.Contains(e.Key) && e.Key > 2))
                                            {
                                                string[] lines = pdfPage.Value.Split('\n');
                                                int lastLinesHasBtwBrief = lines.Count() - 1;
                                                while (lastLinesHasBtwBrief > -1)
                                                {
                                                    if (lines[lastLinesHasBtwBrief].Contains(footerPrefix))
                                                    {
                                                        break;
                                                    }
                                                    lastLinesHasBtwBrief--;
                                                }
                                                if (lastLinesHasBtwBrief + 1 == lines.Count()) continue;
                                                string firstSentence =
                                                    Regex.Replace(lines[lastLinesHasBtwBrief + 1].ToLower(), @"\s+", "")
                                                        .TrimEnd('-');
                                                while (firstSentence.Length <= 40)
                                                {
                                                    firstSentence =
                                                        Regex.Replace(lines[lastLinesHasBtwBrief++ + 1].ToLower(), @"\s+",
                                                            "")
                                                            .TrimEnd('-');
                                                    if (lastLinesHasBtwBrief == lines.Count() - 1) break;
                                                }
                                                if (val.Contains(firstSentence))
                                                {
                                                    //level1.Add(new XElement("pageref", pdfPage.Key));
                                                    var pagerefdiv = new XElement("div",
                                                        new XAttribute("class", "pagerefdiv"));
                                                    var a = new XElement("a",
                                                        new XAttribute("class", "pcalibre pageref pcalibre1"),
                                                        new XAttribute("href", "#backpageref_" + pdfPage.Key),
                                                        new XAttribute("id", "pageref_" + pdfPage.Key),
                                                        new XText(pdfPage.Key.ToString()));
                                                    pagerefdiv.Add(a);
                                                    level4.Add(pagerefdiv);
                                                    collected.Add(pdfPage.Key);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string val = ValueOfElement(level3);
                                        foreach (
                                            var pdfPage in pdfPages.Where(e => !collected.Contains(e.Key) && e.Key > 2))
                                        {
                                            string[] lines = pdfPage.Value.Split('\n');
                                            int lastLinesHasBtwBrief = lines.Count() - 1;
                                            while (lastLinesHasBtwBrief > -1)
                                            {
                                                if (lines[lastLinesHasBtwBrief].Contains(footerPrefix))
                                                {
                                                    break;
                                                }
                                                lastLinesHasBtwBrief--;
                                            }
                                            if (lastLinesHasBtwBrief + 1 == lines.Count()) continue;
                                            string firstSentence =
                                                Regex.Replace(lines[lastLinesHasBtwBrief + 1].ToLower(), @"\s+", "")
                                                    .TrimEnd('-');
                                            while (firstSentence.Length <= 40)
                                            {
                                                firstSentence =
                                                    Regex.Replace(lines[lastLinesHasBtwBrief++ + 1].ToLower(), @"\s+",
                                                        "")
                                                        .TrimEnd('-');
                                                if (lastLinesHasBtwBrief == lines.Count() - 1) break;
                                            }
                                            if (val.Contains(firstSentence))
                                            {
                                                //level1.Add(new XElement("pageref", pdfPage.Key));
                                                var pagerefdiv = new XElement("div",
                                                    new XAttribute("class", "pagerefdiv"));
                                                var a = new XElement("a",
                                                    new XAttribute("class", "pcalibre pageref pcalibre1"),
                                                    new XAttribute("href", "#backpageref_" + pdfPage.Key),
                                                    new XAttribute("id", "pageref_" + pdfPage.Key),
                                                    new XText(pdfPage.Key.ToString()));
                                                pagerefdiv.Add(a);
                                                level3.Add(pagerefdiv);
                                                collected.Add(pdfPage.Key);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string val = ValueOfElement(level2);
                                foreach (var pdfPage in pdfPages.Where(e => !collected.Contains(e.Key) && e.Key > 2))
                                {
                                    string[] lines = pdfPage.Value.Split('\n');
                                    int lastLinesHasBtwBrief = lines.Count() - 1;
                                    while (lastLinesHasBtwBrief > -1)
                                    {
                                        if (lines[lastLinesHasBtwBrief].Contains(footerPrefix))
                                        {
                                            break;
                                        }
                                        lastLinesHasBtwBrief--;
                                    }
                                    if (lastLinesHasBtwBrief + 1 == lines.Count()) continue;
                                    string firstSentence =
                                        Regex.Replace(lines[lastLinesHasBtwBrief + 1].ToLower(), @"\s+", "")
                                            .TrimEnd('-');
                                    while (firstSentence.Length <= 40)
                                    {
                                        firstSentence =
                                            Regex.Replace(lines[lastLinesHasBtwBrief++ + 1].ToLower(), @"\s+", "")
                                                .TrimEnd('-');
                                        if (lastLinesHasBtwBrief == lines.Count() - 1) break;
                                    }
                                    if (val.Contains(firstSentence))
                                    {
                                        //level1.Add(new XElement("pageref", pdfPage.Key));
                                        var pagerefdiv = new XElement("div", new XAttribute("class", "pagerefdiv"));
                                        var a = new XElement("a", new XAttribute("class", "pcalibre pageref pcalibre1"),
                                            new XAttribute("href", "#backpageref_" + pdfPage.Key),
                                            new XAttribute("id", "pageref_" + pdfPage.Key),
                                            new XText(pdfPage.Key.ToString()));
                                        pagerefdiv.Add(a);
                                        level2.Add(pagerefdiv);
                                        collected.Add(pdfPage.Key);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string val = ValueOfElement(level1);
                        foreach (var pdfPage in pdfPages.Where(e => !collected.Contains(e.Key) && e.Key > 2))
                        {
                            string[] lines = pdfPage.Value.Split('\n');
                            int lastLinesHasBtwBrief = lines.Count() - 1;
                            while (lastLinesHasBtwBrief > -1)
                            {
                                if (lines[lastLinesHasBtwBrief].Contains(footerPrefix))
                                {
                                    break;
                                }
                                lastLinesHasBtwBrief--;
                            }
                            if (lastLinesHasBtwBrief + 1 == lines.Count()) continue;
                            string firstSentence =
                                Regex.Replace(lines[lastLinesHasBtwBrief + 1].ToLower(), @"\s+", "").TrimEnd('-');
                            while (firstSentence.Length <= 40)
                            {
                                firstSentence =
                                    Regex.Replace(lines[lastLinesHasBtwBrief++ + 1].ToLower(), @"\s+", "").TrimEnd('-');
                                if (lastLinesHasBtwBrief == lines.Count() - 1) break;
                            }
                            if (val.Contains(firstSentence))
                            {
                                //level1.Add(new XElement("pageref", pdfPage.Key));
                                var pagerefdiv = new XElement("div", new XAttribute("class", "pagerefdiv"));
                                var a = new XElement("a", new XAttribute("class", "pcalibre pageref pcalibre1"),
                                    new XAttribute("href", "#backpageref_" + pdfPage.Key),
                                    new XAttribute("id", "pageref_" + pdfPage.Key), new XText(pdfPage.Key.ToString()));
                                pagerefdiv.Add(a);
                                level1.Add(pagerefdiv);
                                collected.Add(pdfPage.Key);
                                break;
                            }
                        }
                    }
                }
            }
            var divcolofon = new XElement("div", new XAttribute("class", "colofon"));
            var h2 = new XElement("h2", new XAttribute("class", "kopgeghftekst"));
            var span = new XElement("span", new XAttribute("class", "hfteksttitel"), new XAttribute("id", "colofon"), new XText("Colofon"));
            var img = new XElement("img", new XAttribute("src", "../Images/colofon.jpg"), new XAttribute("alt", ""),
                new XAttribute("class", "calibre7"));
            h2.Add(span);
            divcolofon.Add(h2, img);
            body.Add(divcolofon);

            var d = new XElement("div", new XAttribute("class", "hftekst"));
            var a1 = new XElement("a", new XAttribute("href", "../Text/Inhoud.html#paginaregister"),
                new XAttribute("id", "paginaregister"), new XAttribute("class", "vindplaats1"), new XText(" Paginaregister "));
            d.Add(a1);
            foreach (var i in collected)
            {
                var spanpagereglinkdiv = new XElement("span", new XAttribute("class", "pagereglinkdiv"));
                var apagereglinkdiv = new XElement("a", new XAttribute("class", "pagereglink"),
                    new XAttribute("href", "#pageref_" + i), new XAttribute("id", "backpageref_" + i),
                    new XText(i.ToString()));
                spanpagereglinkdiv.Add(apagereglinkdiv);
                d.Add(spanpagereglinkdiv);
            }
            body.Add(d);
            mainSection.Document.Write(Path.Combine(outputPath, "fixRefSection0000.html"), Formatting.Indented);
        }

        private string ValueOfElement(XElement element)
        {
            if (element.Descendants().Any(e => "noot".Equals(e.Name.LocalName)))
            {
                XElement tempCurrent = XElement.Parse(element.ToString());
                tempCurrent.DescendantNodes()
                    .Where(e => e is XElement && "noot".Equals((e as XElement).Name.LocalName))
                    .Select(e => e as XElement)
                    .Elements()
                    .Where(e => "a".Equals(e.Name.LocalName))
                    .Remove();
                return tempCurrent.Value.ToLower().Trim().Replace(" ", "");
            }
            return Regex.Replace(element.Value.ToLower(), @"\s+", "");
        }
    }
}