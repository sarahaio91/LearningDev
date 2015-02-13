using Common;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JournalBUSINESSgsrecht
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string outputPath = @"C:\Epub\jondr\output";
            string jehfccPath = @"C:\Epub\jondr\input\hf-cc_je.xml";
            string jrvhfccPath = @"C:\Epub\jondr\input\hf-cc_jrv.xml";
            new Content().Generate(jrvhfccPath, outputPath, "Journaal Ondernemingsrecht", "jrv");
            new Content().Generate(jehfccPath, outputPath, "Journaal Ondernemingsrecht");
            new Inhoud().Generate(outputPath, "Journaal Ondernemingsrecht");
            new Toc().Generate(outputPath);
            GeneratePageRef(outputPath, 385, 408, "jrv");
            GeneratePageRef(outputPath, 409, 423, "je");
            foreach(var file in Directory.GetFiles(outputPath, "*.*"))
            {
                if (!file.EndsWith(".epub"))
                {
                    CleanUp(file);
                }
            }
        }

        static void GeneratePageRef(string outputPath, int start, int end, string type)
        {
            var body = new XElement("body");
            var tablepageref = new XElement("div", new XAttribute("class", "table"));
            var linkpageref = new XElement("div", new XAttribute("class", "details"));
            for (int i = start; i <= end; i++)
            {
                var spanpagereglinkdiv = new XElement("span", new XAttribute("class", "pagereglinkdiv"));
                var apagereglinkdiv = new XElement("a", new XAttribute("class", "pagereglink"),
                    new XAttribute("href", string.Format("../Text/{0}artikelen.xhtml#pageref_", type) + i), new XAttribute("id", "backpageref_" + i),
                    new XText(i.ToString()));
                spanpagereglinkdiv.Add(apagereglinkdiv);
                tablepageref.Add(spanpagereglinkdiv);

                var pagerefdiv = new XElement("div", new XAttribute("class", "pagerefdiv1"));
                var a = new XElement("a", new XAttribute("class", "pcalibre pageref pcalibre1"),
                    new XAttribute("href", string.Format("../Text/{0}artikelen.xhtml#backpageref_", type) + i),
                    new XAttribute("id", "pageref_" + i),
                    new XText(i.ToString()));
                pagerefdiv.Add(a);
                linkpageref.Add(pagerefdiv);
            }
            body.Add(tablepageref, linkpageref);
            using (
                var writer =
                    new XmlTextWriter(Path.Combine(outputPath, string.Format("pageref_{0}.xml", type)), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                body.Save(writer);
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
            if (modified) File.WriteAllText(file, content.Replace((char)8239, ' '), Encoding.UTF8);
        }
    }
}
