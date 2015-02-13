using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Common
{
    public class InputData
    {
        public string FileName { get; set; }
        public XDocument Document { get; set; }
        public bool IsModified { get; set; }

        public InputData(string fileName)
        {
            this.FileName = fileName;
            var settings = new XmlReaderSettings { ProhibitDtd = false, XmlResolver = new XhtmlResolver() };
            using (var reader = XmlReader.Create(fileName, settings))
            {
                this.Document = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
            }
        }
    }
}
