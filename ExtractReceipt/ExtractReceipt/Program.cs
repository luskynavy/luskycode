using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace ExtractReceipt
{
    internal class Program
    {
        /// <summary>
        /// iText version to extract text from pdf
        /// </summary>
        /// <param name="filePath">pdf file name</param>
        /// <returns>string with text</returns>
        public static string ITextExtractText(string filePath)
        {
            var text = new StringBuilder();
            using (var reader = new iTextSharp.text.pdf.PdfReader(filePath))
            {
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                    //ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();

                    var currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
            }

            return text.ToString();
        }

        /// <summary>
        /// PdfPig version to extract text from pdf
        /// </summary>
        /// <param name="filePath">pdf file name</param>
        /// <returns>string with text</returns>
        public static string PdfPigExtractText(string filePath)
        {
            var text = new StringBuilder();
            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    var currentText = ContentOrderTextExtractor.GetText(page, true);

                    text.Append(currentText);
                }
            }

            return text.ToString();
        }

        private static void Main(string[] args)
        {
            try
            {
                string pdfPath = @"..\..\..\Tickets\";

                var text = PdfPigExtractText(pdfPath + "Ticket de caisse_05082023-140547.pdf");
                var file = new StreamWriter("pdfpig.txt");
                file.Write(text);
                file.Close();

                text = ITextExtractText(pdfPath + "Ticket de caisse_05082023-140547.pdf");
                file = new StreamWriter("itextsharp.txt");
                file.Write(text);
                file.Close();

                Console.WriteLine("Date;Group;Name;Price;Filename;Line");

                var allProducts = new List<Product>();

                var files = Directory.GetFiles(pdfPath, "*.pdf");
                foreach (var pdf in files)
                {
                    //text = ITextExtractText(pdf);

                    text = PdfPigExtractText(pdf);

                    var extractReceiptData = new ExtractReceiptData();
                    extractReceiptData.ExtractData(pdf, text);

                    /*foreach (var product in extractReceiptData.Products.OrderBy(p => p.Name).ThenBy(p => p.DateReceipt))
                    {
                        Console.WriteLine(product.DateReceipt.ToString("yyyy-MM-dd") + ";" + product.SourceName + ";" + product.SourceLine + ";" + product.Group + ";" + product.Name + ";" + product.Price);
                    }*/
                    allProducts.AddRange(extractReceiptData.Products);

                    /*file = new StreamWriter(pdf + ".txt");
                    file.Write(text);
                    file.Close();*/
                }

                foreach (var product in allProducts.OrderBy(p => p.Name).ThenBy(p => p.DateReceipt))
                {
                    Console.WriteLine(product.DateReceipt.ToString("yyyy-MM-dd") + ";" + product.Group + ";" + product.Name + ";" + product.Price + ";" + product.SourceName + ";" + product.SourceLine);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}