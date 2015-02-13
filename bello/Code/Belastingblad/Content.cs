using System.IO;
using System.Xml;
using System.Xml.Linq;
using vzr_common;

namespace Belastingblad
{
    public class Content
    {
        public void Generate(string hfccPath, string hfccxslt, string outputPath)
        {
            var hfcc = XDocument.Load(hfccPath);
            hfcc.XslTransfrom(hfccxslt).Write(Path.Combine(outputPath, "Section0000.html"), Formatting.Indented);

        }
    }
}