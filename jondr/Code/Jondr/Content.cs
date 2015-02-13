using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using vzr_common;

namespace JournalBUSINESSgsrecht
{
    public class Content
    {
        public void Generate(string hfccPath, string outputPath, string title,
            string fileNamePrefix = "je")
        {
            XDocument hfcc = XDocument.Load(hfccPath);
            XElement hf = hfcc.Elements().First(e => "hf".Equals(e.Name.LocalName));
            hf.Add(new XAttribute("title", title));
            XDocument document =
                hfcc.XslTransfrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"xslt\artikelen.xslt"));
            document
                .Write(Path.Combine(outputPath, fileNamePrefix + "artikelen.xhtml"), Formatting.Indented);
        }
    }
}