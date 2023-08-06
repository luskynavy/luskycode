using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using System.Text;

namespace ExtractReceipt
{
    internal class Program
    {
        public static void ITextExtractText(string filePath)
        {
            using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(filePath))
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
            }
        }

        public static void PdfPigExtractText(string filePath)
        {
            using (var document = PdfDocument.Open(filePath))
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("pdfpig.txt");
                foreach (var page in document.GetPages())
                {
                    var text = ContentOrderTextExtractor.GetText(page, true);

                    file.Write(text);
                }

                file.Close();
            }
        }
        static void Main(string[] args)
        {
            string pdfPath = @"Tickets\";
            PdfPigExtractText(pdfPath + "Ticket de caisse_05082023-140547.pdf");

            ITextExtractText(pdfPath + "Ticket de caisse_05082023-140547.pdf");
        }
    }
}