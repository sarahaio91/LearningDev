using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace vzr_common
{
    public class XDocumentSplitter
    {
        public XDocumentSplitter()
        {
            _fileNames = new Queue<string>();
        }

        private int _count = 1;
        private Queue<string> _fileNames;

        public Queue<string> FileNames
        {
            get { return _fileNames; }
        }

        public void SplitFile(XDocument document, string outputFolder, int size = 220000,
            string fileNamePrefix = "Section", string bodyElement = "body", string htmlStructure = @"",
            string entension = ".html")
        {
            if (document.ToString().Length > size)
            {
                if (document.Elements().Any())
                {
                    var nodeStack = new Queue<Tuple<string, IEnumerable<XAttribute>>>();

                    nodeStack.Enqueue(new Tuple<string, IEnumerable<XAttribute>>(bodyElement, new XAttribute[] {}));
                    var xElement = new XElement(bodyElement);
                    Recursive(
                        XElement.Parse(document.Descendants().First(e => "body".Equals(e.Name.LocalName)).ToString()),
                        ref xElement, nodeStack, size, outputFolder, htmlStructure, entension, fileNamePrefix);

                    if (!string.IsNullOrWhiteSpace(xElement.Value))
                    {
                        XDocument section = XDocument.Parse(htmlStructure);
                        section.Descendants().First(e => "body".Equals(e.Name.LocalName)).Add(xElement.Elements());
                        string fileName = fileNamePrefix + (_count++).ToString().PadLeft(4, '0');
                        _fileNames.Enqueue(fileName);
                        section.Write(Path.Combine(outputFolder, fileName + entension));
                    }
                }
                else
                {
                    XDocument section = XDocument.Parse(htmlStructure);
                    section.Descendants().First(e => "body".Equals(e.Name.LocalName)).Add(document.Elements());
                    string fileName = fileNamePrefix + (_count++).ToString().PadLeft(4, '0');
                    _fileNames.Enqueue(fileName);
                    section.Write(Path.Combine(outputFolder, fileName + entension));
                }
            }
            else
            {
                string fileName = fileNamePrefix + (_count++).ToString().PadLeft(4, '0');
                _fileNames.Enqueue(fileName);
                document.Write(Path.Combine(outputFolder, fileName + entension));
            }
        }

        private void Recursive(XElement doc, ref XElement result,
            Queue<Tuple<string, IEnumerable<XAttribute>>> nodeQueue, int size, string outputFolder, string htmlStructure,
            string entension, string fileNamePrefix)
        {
            foreach (XElement element in doc.Elements())
            {
                WriteFileIfY(element, nodeQueue, ref result, outputFolder, htmlStructure, fileNamePrefix);
                if (element.ToString().Length > size)
                {
                    if (element.Elements().Any())
                    {
                        Queue<Tuple<string, IEnumerable<XAttribute>>> queue = nodeQueue.Clone();
                        queue.Enqueue(new Tuple<string, IEnumerable<XAttribute>>(element.Name.LocalName,
                            element.Attributes()));

                        if (nodeQueue.Count > 1)
                        {
                            int deep = 1;
                            XElement lastElement = result;

                            while (deep < nodeQueue.Count)
                            {
                                deep++;
                                lastElement = lastElement.Elements().LastOrDefault();
                            }
                            if (lastElement != null)
                            {
                                lastElement.Add(new XElement(element.Name.LocalName, element.Attributes()));
                            }
                        }
                        else
                        {
                            result.Add(new XElement(element.Name.LocalName, element.Attributes()));
                        }
                        Recursive(element, ref result, queue, size, outputFolder, htmlStructure, entension,
                            fileNamePrefix);
                    }
                    else
                    {
                        XElement tmp = null;
                        Stack<Tuple<string, IEnumerable<XAttribute>>> tempStack = nodeQueue.ToStack();
                        foreach (var stk in tempStack)
                        {
                            if (tmp == null)
                            {
                                tmp = new XElement(stk.Item1, stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));
                            }
                            else
                            {
                                var xElement = new XElement(stk.Item1,
                                    stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));
                                xElement.Add(tmp);
                                tmp = XElement.Parse(xElement.ToString());
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(result.Value))
                        {
                            //result.Save(file + _count++ + extension);

                            XDocument document = XDocument.Parse(htmlStructure);
                            document.Descendants().First(e => "body".Equals(e.Name.LocalName)).Add(result.Elements());
                            //document.Write(Path.Combine(outputFolder, fileNamePrefix + (_count++).ToString().PadLeft(4, '0') + entension));
                            string fileName = fileNamePrefix + (_count++).ToString().PadLeft(4, '0');
                            _fileNames.Enqueue(fileName);
                            document.Write(Path.Combine(outputFolder, fileName + entension));

                            result = XElement.Parse(tmp.ToString());
                        }

                        XElement lastElement = tmp;
                        XElement tmpLast = tmp;
                        while (lastElement != null)
                        {
                            lastElement = lastElement.Elements().LastOrDefault();
                            if (lastElement != null) tmpLast = lastElement;
                        }
                        tmpLast.Add(element);

                        //tmp.Save(file + _count++ + extension);
                        XDocument document1 = XDocument.Parse(htmlStructure);
                        document1.Descendants().First(e => "body".Equals(e.Name.LocalName)).Add(tmp.Elements());
                        string fileName1 = fileNamePrefix + (_count++).ToString().PadLeft(4, '0');
                        _fileNames.Enqueue(fileName1);
                        document1.Write(Path.Combine(outputFolder, fileName1 + entension));
                        //document1.Write(Path.Combine(outputFolder, fileNamePrefix + (_count++).ToString().PadLeft(4, '0') + entension));
                    }
                }
                else
                {
                    XElement temp = XElement.Parse(result.ToString());
                    temp.Add(element);

                    if (temp.ToString().Length > size)
                    {
                        XElement tmp = null;
                        Stack<Tuple<string, IEnumerable<XAttribute>>> tempStack = nodeQueue.ToStack();
                        foreach (var stk in tempStack)
                        {
                            if (tmp == null)
                            {
                                tmp = new XElement(stk.Item1, stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));
                                tmp.Add(element);
                            }
                            else
                            {
                                var xElement = new XElement(stk.Item1,
                                    stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));
                                xElement.Add(tmp);
                                tmp = XElement.Parse(xElement.ToString());
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(result.Value))
                        {
                            XDocument document = XDocument.Parse(htmlStructure);
                            document.Descendants().First(e => "body".Equals(e.Name.LocalName))
                                .Add(result.Elements());

                            string fileName = fileNamePrefix + (_count++).ToString().PadLeft(4, '0');
                            _fileNames.Enqueue(fileName);
                            document.Write(Path.Combine(outputFolder, fileName + entension));

                            result = XElement.Parse(tmp.ToString());
                        }
                    }
                    else
                    {
                        if (nodeQueue.Count > 1)
                        {
                            int deep = 1;
                            XElement lastElement = result;

                            while (deep < nodeQueue.Count)
                            {
                                deep++;
                                lastElement = lastElement.Elements().LastOrDefault();
                            }
                            if (lastElement != null)
                            {
                                lastElement.Add(element);
                            }
                        }
                        else
                        {
                            result.Add(element);
                        }
                    }
                }
            }
        }

        private void WriteFileIfY(XElement element, Queue<Tuple<string, IEnumerable<XAttribute>>> nodeQueue, ref XElement result, string outputFolder, string htmlStructure, string fileNamePrefix)
        {
            var isHoofdstuk =
                element.Elements()
                    .FirstOrDefault(
                        e =>
                            e.Descendants()
                                .Any(
                                    t =>
                                        "span".Equals(t.Name.LocalName) && t.Attribute("class") != null &&
                                        t.Attribute("class").Value.Contains("x1Inhoud")));
                //.Any(e => "span".Equals(e.Name.LocalName) && e.Attribute("class") != null && e.Attribute("class").Value.Contains("x1Inhoud"));
            if (isHoofdstuk != null)
            {
                XElement tmp = null;
                Stack<Tuple<string, IEnumerable<XAttribute>>> tempStack = nodeQueue.ToStack();
                foreach (Tuple<string, IEnumerable<XAttribute>> stk in tempStack)
                {
                    if (tmp == null)
                    {
                        tmp = new XElement(stk.Item1, stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));
                        //tmp = new XElement(stk.Item1, stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));// todo backup
                    }
                    else
                    {
                        var xElement = new XElement(stk.Item1, stk.Item2.Select(a => new XAttribute(a.Name, a.Value)));
                        xElement.Add(tmp);
                        tmp = XElement.Parse(xElement.ToString());
                    }
                }

                // write file.
                if (!string.IsNullOrWhiteSpace(result.Value))
                {
                    var document = XDocument.Parse(htmlStructure);
                    document.Descendants().FirstOrDefault(e => "body".Equals(e.Name.LocalName)).Add(result.Elements());
                    //document.Save(file + _count++ + extension);
                    //using (var writer = new XmlTextWriter(Path.Combine(outputFolder, "Section" + (_count++).ToString().PadLeft(4, '0') + ".html"), Encoding.UTF8))
                    //{
                    //    document.Save(writer);
                    //}
                    string fileName = Path.Combine(outputFolder,
                        fileNamePrefix + (_count++).ToString().PadLeft(4, '0') + ".html");
                    _fileNames.Enqueue(fileName);
                    document.Write(Path.Combine(outputFolder, fileName));

                    result = XElement.Parse(tmp.ToString());
                }
            }
        }
    }
}