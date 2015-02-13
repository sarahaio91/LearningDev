using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Common;

namespace vzr_common
{
    public class Cleaner
    {
        public void CleanUpBr(string xhtmlPath, string fileExtension = ".xhtml")
        {
            IEnumerable<InputData> inputFiles =
                Directory.EnumerateFiles(xhtmlPath, "*" + fileExtension).OrderBy(f => f).Select(f => new InputData(f));
            foreach (InputData inputFile in inputFiles)
            {
                IEnumerable<XElement> invalidBrs = inputFile.Document.Descendants()
                    .Where(
                        e =>
                            "br".Equals(e.Name.LocalName) && e.Attribute("clear") != null &&
                            "none".Equals(e.Attribute("clear").Value));
                if (!invalidBrs.Any()) continue;
                int count = invalidBrs.Count();
                for (int i = count - 1; i > -1; i--)
                {
                    XElement invalidBr = invalidBrs.ElementAt(i);
                    invalidBr.ReplaceWith(new XElement("br"));
                }
                inputFile.Document.Write(Path.Combine(xhtmlPath, Path.GetFileName(inputFile.FileName)));
            }
        }

        public void Clean(string xhtmlPath, string fileExtension = ".xhtml")
        {
            IEnumerable<string> htmlFiles =
                Directory.EnumerateFiles(xhtmlPath, "*" + fileExtension);
            foreach (string htmlFile in htmlFiles)
            {
                string content = File.ReadAllText(htmlFile);
                if (content.Contains(" xmlns=\"\""))
                {
                    content = content.Replace(" xmlns=\"\"", "");
                    File.WriteAllText(htmlFile, content);
                }
            }
        }
    }
}