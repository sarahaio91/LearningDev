using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Belastingblad
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string outputPath = @"C:\Epub\bello\output";
            string hfccPath = @"C:\Epub\bello\input\hf-cc.xml";

            string hfccxslt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\hfcc.xslt");
            new Content().Generate(hfccPath, hfccxslt, outputPath);
            new Inhoud().Generate(outputPath);
            new Toc().Generate(outputPath);
            GeneratePageRef(outputPath, 235, 348);
            CleanUp(Path.Combine(outputPath, "Section0000.html"));
            CleanUp(Path.Combine(outputPath, "Inhoud.html"));
            CleanUp(Path.Combine(outputPath, "toc.ncx"));
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

    public class HotFix
    {
        public void Fix(string xhtmlFolder)
        {
            string path = xhtmlFolder;
            var files = Directory.EnumerateFiles(path, "*.*");
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                content = content.Replace((char)8239, ' ');
                File.WriteAllText(file, content, Encoding.UTF8);
            }
        }

        //TODO: Fix hftekst1 for list under Artikel
    }
}
