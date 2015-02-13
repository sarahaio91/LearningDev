using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ResortNCX
{
    class App
    {
        public void Run(string inputPath, string outputPath)
        {
            var doc = XDocument.Load(inputPath);
            var navPoints = doc.Descendants().Where(i => i.Name.LocalName.Equals("navPoint"));
            var count = 0;
            foreach (var navPoint in navPoints)
            {
                count++;
                navPoint.Attribute("id").Value = count.ToString();
                navPoint.Attribute("playOrder").Value = count.ToString();
            }
            doc.Save(outputPath);
        }
    }
}
