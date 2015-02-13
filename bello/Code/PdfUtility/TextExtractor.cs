using System;
using System.Collections.Generic;
using System.IO;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace PdfUtility
{
    public static class TextExtractor
    {
        public static Dictionary<int, string> Extract(string pdfFileName)
        {
            if (!File.Exists(pdfFileName))
                throw new FileNotFoundException("pdfFileName");

            var result = new Dictionary<int, string>();
            PDDocument pdfDocument = PDDocument.load(pdfFileName);

            var pdfStripper = new PDFTextStripper();
            pdfStripper.setPageSeparator(Environment.NewLine + Environment.NewLine);

            for (int i = 1; i <= pdfDocument.getNumberOfPages(); i++)
            {
                pdfStripper.setStartPage(i);
                pdfStripper.setEndPage(i);

                //ExtractText(pdfStripper, pdfDocument,
                //  string.Format(@"c:\Users\tri.hoang\Desktop\temp\epub-belastingblad\2014-08\pdf\page_{0}.txt", i.ToString().PadLeft(5, '0')));

                result.Add(i, GetText(pdfStripper, pdfDocument));
            }

            pdfDocument.close();
            return result;
        }

        private static void ExtractText(PDFTextStripper textStripper, PDDocument document,
           string outputFile)
        {
            if (File.Exists(outputFile)) File.Delete(outputFile);
            using (var sw = new StreamWriter(outputFile))
            {
                sw.Write(textStripper.getText(document));
            }
        }

        private static string GetText(PDFTextStripper textStripper, PDDocument document)
        {
            return textStripper.getText(document);
        }
    }
}
