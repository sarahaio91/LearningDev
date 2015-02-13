using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfUtility;

namespace PdfTextExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> pdfPages = TextExtractor.Extract(args[0]);
        }
    }
}
