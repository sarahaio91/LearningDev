using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Common;
using vzr_common;

namespace Belastingblad
{
    public class Inhoud
    {
        private const string Template =
            @"<?xml version='1.0' encoding='utf-8'?><html xmlns=""http://www.w3.org/1999/xhtml""><head><title>Belastingblad 7</title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/><link href=""stylesheet.css"" rel=""stylesheet"" type=""text/css""/><link href=""page_styles.css"" rel=""stylesheet"" type=""text/css""/></head><body class=""calibre""><div class=""hftekst""><div class=""balk""><h2 class=""kopgeghftekst"" id=""inhoud""><span class=""hfteksttitel"">Inhoud</span></h2></div><div class=""hftekst""><div class=""vindplaats"" id=""inhoudsopgave"">Inhoudsopgave</div></div></div></body></html>";
        public void Generate(string outputPath)
        {
            var mainSection = new InputData(Path.Combine(outputPath, "Section0000.html"));
            var body = mainSection.Document.Descendants().First(e => "body".Equals(e.Name.LocalName));
            var divs = body.Elements()
                .Where(
                    e =>
                        "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                        "hftekst".Equals(e.Attribute("class").Value));

            bool found = false;
            //var collected = new Dictionary<XElement, List<XElement>>();
            var table = new XElement("table", new XAttribute("class", "calibre1"));
            foreach (var div in divs)
            {
                var h2 = div.Descendants().FirstOrDefault(e => "h2".Equals(e.Name.LocalName));
                found = h2 != null;
                if (found)
                {
                    var tr = new XElement("tr", new XAttribute("class", "calibre2"));
                    var td = new XElement("td", new XAttribute("colspan", "2"), new XAttribute("class", "calibre3"));
                    var a = new XElement("a", new XAttribute("href", "../Text/Section0000.html#"),
                        new XAttribute("id", "back_"),
                        new XAttribute("class", "tochftekst"));
                    a.Value = h2.Value;
                    var h2a = h2.Descendants()
                        .FirstOrDefault(e => "a".Equals(e.Name.LocalName) && e.Attribute("href") != null);
                    a.Attribute("href").Value += h2a.Attribute("id").Value;
                    a.Attribute("id").Value += h2a.Attribute("id").Value;
                    td.Add(a);
                    tr.Add(td);
                    table.Add(tr);
                }
                else
                {
                    var firstdiv = div.Elements().FirstOrDefault();
                    var tr = new XElement("tr", new XAttribute("class", "calibre2"));
                    var td = new XElement("td", new XAttribute("colspan", "2"), new XAttribute("class", "calibre3"));
                    var tempdiv = new XElement("div", new XAttribute("class", "hftekst"));
                    var a = new XElement("a", new XAttribute("href", "../Text/Section0000.html#"),
                        new XAttribute("id", "back_"),
                        new XAttribute("class", "toclink"));
                    var firstdiva = firstdiv.Descendants()
                        .FirstOrDefault(e => "a".Equals(e.Name.LocalName) && e.Attribute("id") != null);
                    a.Value =  firstdiva.Value.Trim();
                    a.Attribute("href").Value += firstdiva.Attribute("id").Value;
                    a.Attribute("id").Value += firstdiva.Attribute("id").Value;
                    var instantienaam = firstdiv.Descendants()
                        .FirstOrDefault(
                            e =>
                                "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                                "instantienaam".Equals(e.Attribute("class").Value));
                    tempdiv.Add(a);
                    if (instantienaam != null)
                    {
                        var div1 = new XElement("div", new XAttribute("class", "hftekst"));
                        div1.Value = instantienaam.ElementsAfterSelf()
                            .ElementAt(0)
                            .Value.Substring(0, instantienaam.ElementsAfterSelf().ElementAt(0).Value.IndexOf(','));
                        tempdiv.Add(div1);
                        var essentieromp = firstdiv.ElementsAfterSelf().First().Descendants().
                            Where(e => "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                                    "essentieromp".Equals(e.Attribute("class").Value)).FirstOrDefault(); 
                        if(essentieromp != null)
                        {
                            var temaa = firstdiv.ElementsAfterSelf()
                            .First()
                            .Descendants()
                            .First(
                                e =>
                                    "div".Equals(e.Name.LocalName) && e.Attribute("class") != null &&
                                    "essentieromp".Equals(e.Attribute("class").Value));
                            var div2 = new XElement("div", new XAttribute("class", "hftekst"));
                            div2.Value = temaa.Value;
                            tempdiv.Add(div2);
                        }
                    }
                    td.Add(tempdiv);
                    tr.Add(td);
                    table.Add(tr);
                }
            }
            var ttr = new XElement("tr", new XAttribute("class", "calibre2"));
            var ttd = new XElement("td", new XAttribute("colspan", "2"), new XAttribute("class", "calibre3"));
            var ixhftekst = new XElement("div", new XAttribute("class", "ixhftekst"));
            var aa = new XElement("a", new XAttribute("href", "../Text/Section0000.html#paginaregister"),
                new XAttribute("id", "paginaregister"),
                new XAttribute("class", "hfteksttitel"), new XText("Paginaregister"));
            ixhftekst.Add(aa);
            ttd.Add(ixhftekst);
            ttr.Add(ttd);
            table.Add(ttr);

            var template = XDocument.Parse(Template);
            var inhoudsopgave = template.Descendants()
                .First(
                    e =>
                        "div".Equals(e.Name.LocalName) && e.Attribute("id") != null &&
                        "inhoudsopgave".Equals(e.Attribute("id").Value));
            inhoudsopgave.AddAfterSelf(table);
            template.Write(Path.Combine(outputPath, "Inhoud.html"), Formatting.Indented);
        }
    }
}