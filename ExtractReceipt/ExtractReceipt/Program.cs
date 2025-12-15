using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using System.Text;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
                bool noCsv = false;
                bool mysqlMode = false;
                bool noAddMode = false;

                //Manage options
                foreach (var arg in args)
                {
                    if (arg == "-nocsv")
                    {
                        noCsv = true;
                    }
                    else if (arg == "-mysql")
                    {
                        mysqlMode = true;
                    }
                    else if(arg == "-noadd")
                    {
                        noAddMode = true;
                    }
                }

                //Extract products from pdf
                Console.Write("ExtractProducts");
                Stopwatch sw = Stopwatch.StartNew();
                var allProducts = ExtractProducts(pdfPath);
                sw.Stop();
                Console.WriteLine($" in {sw.ElapsedMilliseconds} ms");

                //Export csv if needed
                if (!noCsv)
                {
                    Console.Write("ExportCsv");
                    sw.Start();
                    ExportCsv("receipts.csv", allProducts);
                    sw.Stop();
                    Console.WriteLine($" in {sw.ElapsedMilliseconds} ms");
                }

                // Don't add products to database in noAddMode
                if (!noAddMode)
                {
                    //Add products do db
                    Console.Write("AddProductsToDb" + (mysqlMode ? " MySql" : ""));
                    sw.Start();
                    int nbProductsAdded;
                    if (!mysqlMode)
                    {
                        nbProductsAdded = AddProductsToDb(allProducts);
                    }
                    else
                    {
                        nbProductsAdded = AddProductsToDbMysql(allProducts);
                    }
                    sw.Stop();
                    Console.WriteLine($" added {nbProductsAdded} product(s) in {sw.ElapsedMilliseconds} ms");
                }

                Console.WriteLine("Done");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Extract products from pdf.
        /// </summary>
        /// <param name="pdfPath">path with pdf</param>
        /// <returns>list of all products</returns>
        private static List<Product> ExtractProducts(string pdfPath)
        {
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

            return allProducts;
        }

        /// <summary>
        /// Export products in csv file.
        /// </summary>
        /// <param name="csvName">csv file name</param>
        /// <param name="allProducts">list of products</param>
        private static void ExportCsv(string csvName, List<Product> allProducts)
        {
            var file = new StreamWriter(csvName);
            file.WriteLine("Date;Group;Name;Price;Filename;Line;FullData;PriceDiff");

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

        /// <summary>
        /// Add all the products to the db
        /// </summary>
        /// <param name="allProducts">products list</param>
        private static int AddProductsToDb(List<Product> allProducts)
        {
            //To create DB on Package Manager Console:
            //Add - Migration InitialMigration
            //Update-Database

            int nbProductsAdded = 0;

            //Init db.
            using (var dbContext = new ApplicationDbContext())
            {
                //Empty products in db.
                //dbContext.Products.ExecuteDelete();

                //Add all products to db.
                //dbContext.Products.AddRange(allProducts);

                foreach(var product in allProducts)
                {
                    //Add product only if sourcename is not found
                    if (!dbContext.Products.Any(p => p.SourceName == product.SourceName
                    && p.SourceLine == product.SourceLine))
                    {
                        dbContext.Products.Add(product);
                        nbProductsAdded++;
                    }
                }

                if (nbProductsAdded != 0)
                {
                    dbContext.SaveChanges();
                }
            }

            return nbProductsAdded;
        }

        /// <summary>
        /// Add all the products to the MySql db
        /// </summary>
        /// <param name="allProducts">products list</param>
        private static int AddProductsToDbMysql(List<Product> allProducts)
        {
            int nbProductsAdded = 0;

            //Init db.
            using (var dbContext = new MysqlDbContext())
            {

                foreach (var product in allProducts)
                {
                    //Add product only if sourcename is not found
                    if (!dbContext.Products.Any(p => p.SourceName == product.SourceName
                    && p.SourceLine == product.SourceLine))
                    {
                        dbContext.Products.Add(product);
                        nbProductsAdded++;
                    }
                }

                if (nbProductsAdded != 0)
                {
                    dbContext.SaveChanges();
                }
            }

            return nbProductsAdded;
        }
    }
}