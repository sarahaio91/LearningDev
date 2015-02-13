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
        public XDocument FixPageRef(string hfccFilePath, string pdfFilePath, string footerPrefix)
        {
            var hfcc = new InputData(hfccFilePath);
            IEnumerable<XElement> sections = SplitSection(hfcc);

            if (sections.Any())
            {

                Dictionary<int, string> pdfPages = TextExtractor.Extract(pdfFilePath);
                var collected = new HashSet<int>();

                foreach (XElement section in sections)
                {
                    XElement kenmerkgrp = section.Element("kenmerkgrp");
                    if (kenmerkgrp == null) continue;
                    XElement kenmerk = kenmerkgrp.Element("kenmerk");
                    if (kenmerk == null) continue;
                    XElement commentaarcontent = section.Element("commentaarcontent");
                    if (commentaarcontent == null) continue;
                    XElement verhandelingalgemeen = commentaarcontent.Element("verhandelingalgemeen");
                    if (verhandelingalgemeen == null) continue;

                    string query = footerPrefix + kenmerk.Value.Trim();

                    var pagesNo = new Dictionary<int, string>();
                    foreach (var pdfPage in pdfPages)
                    {
                        if (pdfPage.Value.Contains(query))
                        {
                            pagesNo[pdfPage.Key] = pdfPage.Value;
                        }
                    }
                    XElement samenvatting = verhandelingalgemeen.Element("samenvatting");
                    foreach (var pageNo in pagesNo)
                    {
                        string[] lines = pageNo.Value.Split('\n');
                        int lastLinesHasBtwBrief = lines.Count() - 1;
                        while (lastLinesHasBtwBrief > -1)
                        {
                            if (lines[lastLinesHasBtwBrief].Contains(query))
                            {
                                break;
                            }
                            lastLinesHasBtwBrief--;
                        }
                        if (lastLinesHasBtwBrief + 1 == lines.Count()) continue;
                        string firstSentence =
                            lines[lastLinesHasBtwBrief + 1].Trim().TrimEnd('-').Replace(" ", "").ToLower();
                        if (samenvatting != null &&
                            ValueOfElement(samenvatting).Contains(firstSentence))
                        {
                            samenvatting.Add(new XElement("a", new XAttribute("type", "pageref"), pageNo.Key));
                            collected.Add(pageNo.Key);
                        }
                        else
                        {
                            bool found = false;
                            IEnumerable<XElement> commentaarcontentabloks = verhandelingalgemeen.Elements("ablok");
                            foreach (XElement commentaarcontentablok in commentaarcontentabloks)
                            {
                                if (ValueOfElement(commentaarcontentablok).Contains(firstSentence))
                                {
                                    commentaarcontentablok.Add(new XElement("a", new XAttribute("type", "pageref"),
                                        pageNo.Key));
                                    collected.Add(pageNo.Key);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                IEnumerable<XElement> ps = verhandelingalgemeen.Elements("p");
                                bool b = false;
                                foreach (XElement p in ps)
                                {
                                    IEnumerable<XElement> pAbloks =
                                        p.Descendants().Where(e => "ablok".Equals(e.Name.LocalName));
                                    foreach (XElement pAblok in pAbloks)
                                    {
                                        if (ValueOfElement(pAblok).Contains(firstSentence))
                                        {
                                            pAblok.Add(new XElement("a", new XAttribute("type", "pageref"), pageNo.Key));
                                            collected.Add(pageNo.Key);
                                            b = true;
                                            break;
                                        }
                                    }
                                    if (b)
                                        break;
                                }
                            }
                        }
                    }
                }

                IEnumerable<int> outer = pdfPages.Keys.Where(k => !collected.Contains(k));
                if (outer.Any())
                {
                    Console.WriteLine("Couldn't add page ref: " + string.Join(", ", outer));
                    Debug.WriteLine("Couldn't add page ref: " + string.Join(", ", outer));
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
