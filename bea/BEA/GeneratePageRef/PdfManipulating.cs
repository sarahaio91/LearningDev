using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common;
using PdfUtility;
using System.Diagnostics;

namespace GeneratePageRef
{
    public class PdfManipulating
    {
        public XDocument FixPageRef(string hfccFilePath, string pdfFilePath, int footer)
        {
            var hfcc = new InputData(hfccFilePath);
            IEnumerable<XElement> sections = SplitSection(hfcc);

            if (sections.Any())
            {
                foreach (XElement section in sections)
                {
                    var vindplaatsred = section.Descendants().Where(i => i.Name.LocalName.Equals("vindplaatsred")).FirstOrDefault();
                    if (vindplaatsred == null) continue;
                    vindplaatsred.Add(new XElement("a", new XAttribute("type", "pageref"), footer));
                    footer++;
                }
            }
            return hfcc.Document;
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
            return element.Value.ToLower().Trim().Replace(" ", "");
        }

        private IEnumerable<XElement> SplitSection(InputData hfcc)
        {
            return hfcc.Document.Descendants().Where(e => "pe".Equals(e.Name.LocalName));
        }
    }
}
