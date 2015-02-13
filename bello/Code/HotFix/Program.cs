using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace HotFix
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = new XmlDocument();
            var newFile = Path.Combine(args[1], "Details.html");
            if (File.Exists(newFile)) File.Delete(newFile);
            using (var tr = new XmlTextReader(new StreamReader(args[0])))
            {
                tr.XmlResolver = null;
                doc.Load(tr);
                if (doc.DocumentType != null)
                {
                    var docType = doc.DocumentType;
                    doc.RemoveChild(docType);
                }
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(newFile, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    doc.Save(xmlTextWriter);
                }
            }
            using (Stream strm = Assembly.GetExecutingAssembly().GetManifestResourceStream("HotFix.hotfix.xslt"))
            using (XmlReader xsl = XmlReader.Create(strm))
            {
                var transform = new XslTransform();
                transform.Load(xsl);
                transform.Transform(newFile, newFile);
            }
            Update(newFile);
        }

        static void Update(string file)
        {
            var doc = new InputData(file);
            var modified = false;
            FixKopgeg(doc, ref modified);
            FixFootNote(doc, ref modified);
            FixHftekst1(doc, ref modified);
            FixFootnoteLayout(doc, ref modified);
            if (modified)
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(file, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    doc.Document.Save(xmlTextWriter);
                }
                CleanUp(file);
            }
        }

        static void FixKopgeg(InputData doc, ref bool modified)
        {
            var auteurgegs = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("div") &&
                i.Attribute("class") != null && i.Attribute("class").Value.Equals("auteurgeg"));
            foreach (var auteurgeg in auteurgegs)
            {
                var firstNode = auteurgeg.Nodes().FirstOrDefault();
                while(firstNode != null && !(firstNode is XElement))
                {
                    firstNode = firstNode.NextNode;
                }
                if (firstNode != null && firstNode is XElement)
                {
                    var firstElement = firstNode as XElement;
                    if (firstElement.Name.LocalName.Equals("kopgeg"))
                    {
                        var hftekst = auteurgeg.Descendants().Where(i => i.Name.LocalName.Equals("div") &&
                            i.Attribute("class") != null && i.Attribute("class").Value.Equals("hftekst")).FirstOrDefault();
                        if (hftekst != null)
                        {
                            hftekst.AddFirst(new XText(string.Format("{0} ", firstElement.Value.Trim())));
                            firstElement.SetAttributeValue("delete", "true");
                            modified = true;
                        }
                    }
                }
            }
            auteurgegs.Descendants().Where(i => i.Name.LocalName.Equals("kopgeg") &&
                i.Attribute("delete") != null).Remove();
        }

        static void FixFootNote(InputData doc, ref bool modified)
        {
            var vnrefcontents = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("td") &&
                i.Attribute("class") != null && i.Attribute("class").Value.Equals("vnrefcontent"));
            foreach (var vnrefcontent in vnrefcontents)
            {
                var firstNode = vnrefcontent.Nodes().FirstOrDefault();
                while (firstNode != null && !(firstNode is XElement))
                {
                    firstNode = firstNode.NextNode;
                }
                if (firstNode != null && firstNode is XElement)
                {
                    var firstElement = firstNode as XElement;
                    if (firstElement.Name.LocalName.Equals("kopgeg"))
                    {
                        var preNode = vnrefcontent.PreviousNode;
                        while (preNode != null && !(preNode is XElement))
                        {
                            preNode = preNode.PreviousNode;
                        }
                        if (preNode != null && preNode is XElement)
                        {
                            var preElement = preNode as XElement;
                            if(preElement.Name.LocalName.Equals("td") && preElement.Attribute("class") != null &&
                                preElement.Attribute("class").Value.Equals("vnreflink"))
                            {
                                var link = preElement.Descendants().Where(i => i.Name.LocalName.Equals("a") &&
                                    i.Attribute("class") != null && i.Attribute("class").Value.Equals("toclink") &&
                                    i.Attribute("href") != null).FirstOrDefault();
                                if(link != null)
                                {
                                    var id = link.Attribute("href").Value.Replace("#", "");
                                    if(!string.IsNullOrEmpty(id))
                                    {
                                        var detailLink = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("a") &&
                                            i.Attribute("class") != null && i.Attribute("class").Value.Equals("toclink") &&
                                            i.Attribute("id") != null && i.Attribute("id").Value.Equals(id)).FirstOrDefault();
                                        if (detailLink != null)
                                        {
                                            detailLink.SetValue(firstElement.Value.Trim());
                                            link.SetValue(firstElement.Value.Trim());
                                            firstElement.SetAttributeValue("delete", "true");
                                            modified = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            vnrefcontents.Descendants().Where(i => i.Name.LocalName.Equals("kopgeg") &&
                i.Attribute("delete") != null).Remove();
        }

        static void FixHftekst1(InputData doc, ref bool modified)
        {
            var hftekst1s = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("div") &&
                i.Attribute("class") != null && i.Attribute("class").Value.Equals("hftekst1"));
            foreach (var hftekst1 in hftekst1s)
            {
                var firstNode = hftekst1.Nodes().FirstOrDefault();
                if (firstNode != null && firstNode is XText && firstNode.ToString().Contains(" "))
                {
                    var number = firstNode.ToString().Split(' ').Where(i => !string.IsNullOrEmpty(i)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(number))
                    {
                        var nextNode = hftekst1.NextNode;
                        while (nextNode != null && !(nextNode is XElement))
                        {
                            nextNode = nextNode.NextNode;
                        }
                        if (nextNode != null && nextNode is XElement)
                        {
                            var nextEl = nextNode as XElement;
                            if (nextEl.Name.LocalName.Equals("div") && nextEl.Attribute("class") != null &&
                                nextEl.Attribute("class").Value.Equals("hftekst"))
                            {
                                var first = nextEl.Nodes().FirstOrDefault();
                                if (first != null && first is XText)
                                {
                                    first.ReplaceWith(new XText(string.Format("{0} {1}", number, first.ToString())));
                                    hftekst1.SetAttributeValue("delete", "true");
                                    modified = true;
                                }
                            }
                        }
                    }
                }
            }
            hftekst1s.Where(i => i.Attribute("delete") != null).Remove();
        }

        static void FixFootnoteLayout(InputData doc, ref bool modified)
        {
            var cells = doc.Document.Descendants().Where(i => i.Name.LocalName.Equals("td") && i.Attribute("class") != null &&
                i.Attribute("class").Value.Equals("vnrefcontent"));
            foreach (var cell in cells)
            {
                var hftekst = cell.Descendants().Where(i => i.Name.LocalName.Equals("div") && i.Attribute("class") != null &&
                    i.Attribute("class").Value.Equals("hftekst")).FirstOrDefault();
                if (hftekst != null)
                {
                    var firstNode = hftekst.Nodes().FirstOrDefault();
                    if (firstNode != null && firstNode is XText)
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(firstNode.ToString().TrimStart(), @"^[\d]{1,2}[.]\s");
                        if (match.Success)
                        {
                            firstNode.ReplaceWith(new XText(firstNode.ToString().TrimStart().Replace(match.Value, "")));
                            modified = true;
                        }
                    }
                }
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
