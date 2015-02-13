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

                result.Add(i, GetText(pdfStripper, pdfDocument));
            }

            pdfDocument.close();
            return result;
        }

        private static string GetText(PDFTextStripper textStripper, PDDocument document)
        {
            return textStripper.getText(document);
        }
    }
}
