using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using System.Text;

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

                var file = new StreamWriter("receipts.csv");
                file.WriteLine("Date;Group;Name;Price;Filename;Line;FullData;PriceDiff");

                var allProducts = new List<Product>();

                var files = Directory.GetFiles(pdfPath, "*.pdf");
                foreach (var pdf in files)
                {
                    /*var text = ITextExtractText(pdf);
                    text = text.Replace("\n", "\r\n");*/
                    var text = PdfPigExtractText(pdf);

                    var extractReceiptData = new ExtractReceiptData();
                    extractReceiptData.ExtractData(pdf, text);

                    //Add the products to the list of all products.
                    if (extractReceiptData.Products != null)
                    {
                        allProducts.AddRange(extractReceiptData.Products);
                    }
                }

                string previousProductName = "";
                decimal previousPrice = 0m;
                //Write products sorted by name then date.
                foreach (var product in allProducts.OrderBy(p => p.Name).ThenBy(p => p.DateReceipt))
                {
                    file.Write(product + ";");
                    //Write price difference with previous line if product is the same.
                    if (previousProductName != "" && previousProductName == product.Name)
                    {
                        file.Write(decimal.Round(product.Price / (previousPrice != 0 ? previousPrice : 1m), 2));
                    }
                    file.WriteLine();

                    previousProductName = product.Name ?? "";
                    previousPrice = product.Price;
                }

                file.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}