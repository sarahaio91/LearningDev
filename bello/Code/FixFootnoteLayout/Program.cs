using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FixFootnoteLayout
{
    class Program
    {
        static void Main(string[] args)
        {
            List<InputData> sections = (
                from f in Directory.EnumerateFiles(args[0], "*.html")
                orderby f
                where Path.GetFileName(f).StartsWith("Section")
                select new InputData(f)).ToList<InputData>();

            foreach (var section in sections)
            {
                var modified = false;
                var cells = section.Document.Descendants().Where(i => i.Name.LocalName.Equals("td") && i.Attribute("class") != null &&
                    i.Attribute("class").Value.Equals("vnrefcontent"));
                foreach(var cell in cells)
                {
                    var hftekst = cell.Descendants().Where(i => i.Name.LocalName.Equals("div") && i.Attribute("class") != null &&
                        i.Attribute("class").Value.Equals("hftekst")).FirstOrDefault();       
                    if(hftekst != null)
                    {
                        var firstNode = hftekst.Nodes().FirstOrDefault();
                        if(firstNode != null && firstNode is XText)
                        {
                            var match = Regex.Match(firstNode.ToString().TrimStart(), @"^[\d]{1,2}[.]\s");
                            if (match.Success)
                            {
                                firstNode.ReplaceWith(new XText(firstNode.ToString().TrimStart().Replace(match.Value, "")));
                                modified = true;
                            }
                        }
                    }
                }
                if (modified)
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(Path.Combine(args[1], Path.GetFileName(section.FileName)), Encoding.UTF8))
                    {
                        xmlTextWriter.Formatting = Formatting.Indented;
                        section.Document.Save(xmlTextWriter);
                    }
                }
            }

            foreach (var file in Directory.EnumerateFiles(args[1], "*.html"))
            {
                CleanUp(file);
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
