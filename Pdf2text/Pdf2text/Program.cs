using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;

using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;

using PdfToText;

using PdfTextract;

namespace Pdf2text
{
    class Program
    {
        //PdfSharp version
        public static IEnumerable<string> ExtractText(PdfSharp.Pdf.PdfPage page)
        {
            CObject content = ContentReader.ReadContent(page);
            var text = ExtractText(content);
            return text;
        }

        private static IEnumerable<string> ExtractText(CObject cObject)
        {
            var textList = new List<string>();
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                    {
                        textList.AddRange(ExtractText(cOperand));
                    }
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                {
                    textList.AddRange(ExtractText(element));
                }
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                textList.Add(cString.Value);
            }
            return textList;
        }

        static void Main(string[] args)
        {
            //using (PdfReader reader = new PdfReader("letter.pdf")) //Index was outside the bounds of the array.
            //using (PdfReader reader = new PdfReader("employe-1.pdf")) //ok
            //using (PdfReader reader = new PdfReader("feuille_de_paie.pdf"))  //Rebuild failed: trailer not found.; Original message: PDF startxref not found.          
            using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader("modele-bulletin-de-salaire.pdf")) //ok
            {
                StringBuilder text = new StringBuilder();

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                    //ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();

                    string currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, page, strategy);             

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }

                System.IO.StreamWriter file = new System.IO.StreamWriter("itextsharp.txt");
                file.WriteLine(text);

                file.Close();

                //return text.ToString();
            }

            //PdfSharp
            using (var _document = PdfReader.Open("letter.pdf", PdfDocumentOpenMode.ReadOnly)) //ok
            //using (var _document = PdfReader.Open("employe-1.pdf", PdfDocumentOpenMode.ReadOnly)) //ok
            //using (var _document = PdfReader.Open("feuille_de_paie.pdf", PdfDocumentOpenMode.ReadOnly)) //Non-negative number required.
            //using (var _document = PdfReader.Open("modele-bulletin-de-salaire.pdf", PdfDocumentOpenMode.ReadOnly))  //ok            
            {
                
                System.IO.StreamWriter file = new System.IO.StreamWriter("pdfsharp.txt");
                foreach (PdfPage page in _document.Pages)
                {
                    var text = ExtractText(page);

                    foreach (string s in text)
                    {
                        file.Write(s);
                    }                    
                }
                file.Close();
            }

            //iTextSharp too
            PDFParser parser = new PDFParser();
            //parser.ExtractText("letter.pdf", "pdfparser.txt"); //error
            parser.ExtractText("employe-1.pdf", "pdfparser.txt"); //ok
            //parser.ExtractText("feuille_de_paie.pdf", "pdfparser.txt"); //error
            //parser.ExtractText("modele-bulletin-de-salaire.pdf", "pdfparser.txt"); //error

            //PdfSharp
            //string text2 = PdfTextExtractor.GetText("letter.pdf"); //ok
            //string text2 = PdfTextExtractor.GetText("employe-1.pdf"); //ok
            //string text2 = PdfTextExtractor.GetText("feuille_de_paie.pdf"); //Non-negative number required.
            string text2 = PdfTextExtractor.GetText("modele-bulletin-de-salaire.pdf"); //ok
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("PdfTextExtractor.txt");
            file2.Write(text2);
            file2.Close();
        }
    }
}
