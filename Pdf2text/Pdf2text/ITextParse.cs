using System;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ReadPdf
{
    public class ITextParse
    {
        public static void ExtractText(string fileName, string outFileName)
        {
            /* pour extraire seulement les 3 1ères pages
             PdfReader pdfReader1 = new PdfReader("AN_PAI_12022018_1M.pdf");
            Document document = new Document();
            
            PdfCopy copy = new PdfCopy(document, new FileStream("splitpaie1-3.pdf", FileMode.Create));
            document.Open();
            for (int page = 1; page <= 3; page++)
            {
                document.NewPage();
                copy.AddPage(copy.GetImportedPage(pdfReader1, page));
            }
            document.Close();*/


            StreamWriter outFile = new StreamWriter(outFileName, false, System.Text.Encoding.UTF8);
            StreamWriter outFile2 = new StreamWriter("filtered " + outFileName, false, System.Text.Encoding.UTF8);

            PdfReader pdfReader = new PdfReader(fileName);            
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                Rectangle psize = pdfReader.GetPageSize(page);

                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                //ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();

                string id1, id2, netRegex, netLocation, idLocation;
                netRegex = id1 = id2 = "";
                string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                outFile.Write(currentPageText);
                var match = Regex.Match(currentPageText, "Matricule : *([0-9]*)  SS : ([0-9]*)");
                if (match.Success)
                {
                    id1 = match.Groups[1].Value;
                    id2 = match.Groups[2].Value;
                    outFile2.Write("Matricule : " + id1 + "  SS : " + id2);
                }
                outFile2.Write(";");

                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(400, 842 - 128, 480, 848 - 110);
                RenderFilter[] renderFilter = new RenderFilter[1];
                renderFilter[0] = new RegionTextRenderFilter(rect);
                ITextExtractionStrategy textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
                idLocation = PdfTextExtractor.GetTextFromPage(pdfReader, page, textExtractionStrategy);
                outFile2.Write(";");
                outFile2.Write(idLocation);
                outFile2.Write(";");

                if ("Matricule : " + id1 + "  SS : " + id2 != idLocation.Trim())
                {
                    int h = 0;
                }

                //match = Regex.Match(currentPageText, "Pér *([0-9]*) *([0-9]*) *([0-9]*)");
                match = Regex.Match(currentPageText, "Net imposable(.*)Net imposable");
                if (match.Success)
                {
                    netRegex = match.Groups[1].Value;
                    outFile2.Write(netRegex);
                }

                rect = new iTextSharp.text.Rectangle(150, 240, 210, 250);
                renderFilter = new RenderFilter[1];
                renderFilter[0] = new RegionTextRenderFilter(rect);
                textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
                netLocation = PdfTextExtractor.GetTextFromPage(pdfReader, page, textExtractionStrategy);


                rect = new iTextSharp.text.Rectangle(400, 842 - 128, 480, 848 - 110);
                renderFilter = new RenderFilter[1];
                renderFilter[0] = new RegionTextRenderFilter(rect);
                textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategyEx(), renderFilter);
                string matriculeLocation = PdfTextExtractor.GetTextFromPage(pdfReader, page, textExtractionStrategy);

                outFile2.Write(";");
                outFile2.Write(netLocation);

                if (netRegex.Trim() != netLocation.Trim())
                {
                    int x = 0;
                    //page 259, rappel période antérieure, pas de net imposable de période
                }

                if (netLocation == "")
                {
                    int z = 0;
                    //page 56, normal, sur plusieurs pages
                }

                outFile2.WriteLine();                
            }

            pdfReader.Close();
            outFile.Close();
            outFile2.Close();
        }
    }
}