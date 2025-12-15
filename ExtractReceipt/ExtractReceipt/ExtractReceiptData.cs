using System.Text.RegularExpressions;

namespace ExtractReceipt
{
    public partial class ExtractReceiptData
    {
        // List of products.
        public List<Product>? Products { get; set; }

        // Begin of products format markers
        private const string _oldFormatMarker = ">>>>";
        private const string _newFormatMarker = "*** VENTE ***";

        // End of products format makers
        private const string _oldEndMarker = "===";
        private const string _newEndMarker = "Nombre de lignes d'article";

        /// <summary>
        /// Extract products list from the text.
        /// </summary>
        /// <param name="pdfName">source file name</param>
        /// <param name="text">text with all data</param>
        public void ExtractData(string pdfName, string text)
        {
            Products = new List<Product>();

            var lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            if (lines != null)
            {
                // Search for old format marker
                int oldFormatIndex = Array.FindIndex(lines, l => l.StartsWith(_oldFormatMarker));

                if (oldFormatIndex != -1)
                {
                    OldFormat(pdfName, lines, oldFormatIndex);
                }
                else
                {
                    // Search for new format marker
                    int newMarkerIndex = Array.FindIndex(lines, l => l.StartsWith(_newFormatMarker));

                    if (newMarkerIndex != -1)
                    {
                        NewFormat(pdfName, lines, newMarkerIndex);
                    }
                    else
                    {
                        Console.WriteLine($"\nUnkown format for {pdfName}");
                    }

                }
            }
        }

        /// Extract products from lines in Products, old format version
        /// </summary>
        /// <param name="pdfName">name of the pdf</param>
        /// <param name="lines">array of lines</param>
        /// <param name="markerIndex">index after marker where products starts</param>
        private void OldFormat(string pdfName, string[] lines, int markerIndex)
        {

            var dateReceipt = new DateTime();
            var productsFound = false;
            var productsGroup = "";

            for (var i = markerIndex; i < lines.Length; i++)
            {
                // Search the receipt date.
                if (lines[i].Contains("Date"))
                {
                    // Next line contain the date.
                    dateReceipt = ExtractDate(lines, i);

                    // Skip the line with date.
                    i++;
                }

                // Search the products start.
                if (lines[i].Trim().StartsWith(_oldFormatMarker))
                {
                    productsFound = true;
                    productsGroup = lines[i].Trim()[4..].Trim();
                }
                else if (productsFound)
                {
                    /*
                     * Differents cases
                     * A) A group with one product, one line with price:
                     *  >>>> LAITS ET DERIVES
                     *  LAIT 1/2 EC.PPX BRIQUE 1L        (T)        0,78 €  11
                     *  >>>> TRAITEUR LS UVCI
                     *
                     * A group with different products, one line by product with its price:
                     *  >>>> FROMAGE LS
                     *  REBLOC AOP CRU 27% POCHAT450G               5,82 €  11
                     *  EMM.PAST.28%MG U PORTION 400G               3,09 €  11
                     *  >>>> TRAITEUR LS UVCI
                     *
                     * B) A product on two lines, price on first line, " x " on second line, price by kg/L on second line:
                     *  >>>> FRUITS
                     *  POMME GOLDEN DELICIOUS                      1,51 €  11
                     *  0,892 kg  x     1,69 €/kg
                     *  >>>> PATIS.INDUSTRIELLE
                     *
                     * C) A product on two lines, no price on first line, " x " on second line, price on second line:
                     *  >>>> LAITS ET DERIVES
                     *  LAIT 1/2 EC.PPX BRIQUE 1L            (T)
                     *  2 x     0,74 €                          1,48 €  11
                     *  >>>> SURGELE SALE
                     *
                     * A product on two lines, no price on first line, " x " on second line, price on second line:
                     *  >>>> FRUITS
                     *  POMME GOLDEN DELICIOUS PILAT      (T)               11
                     *  0,898 kg  x     2,69 €/kg         2,42 €
                     *  >>>> SURGELE SALE
                     *
                     * A product on two lines, no price on first line, " x " on second line, price on second line:
                     *  >>>> LAITS ET DERIVES
                     *  LAIT 1/2 EC.PPX BRIQUE 1L
                     *  2 x     0,74 €                          1,48 €  11
                     *  >>>> FROMAGE LS
                     *
                     */

                    // Price on 1st line: : case A or B.
                    if (lines[i].Contains('€'))
                    {
                        var name = ExtractProductName(lines[i].Trim());
                        // Case A : one product with its price on one line.
                        if (!lines[i + 1].Contains(" x "))
                        {
                            var product = new Product()
                            {
                                Name = name,
                                Price = ExtractProductPrice(lines[i].Trim(), true),
                                Group = productsGroup,
                                DateReceipt = dateReceipt,
                                SourceName = pdfName,
                                SourceLine = i,
                                FullData = lines[i].Trim()
                            };

                            // Lines with *** are for cancellation, don't add
                            if (!name.Contains("***"))
                            {
                                Products.Add(product);
                            }
                        }
                        // Case B: second line with price by kg/L/product.
                        else
                        {
                            var product = new Product()
                            {
                                Name = name,
                                Price = ExtractProductPrice(lines[i + 1].Trim(), false),
                                Group = productsGroup,
                                DateReceipt = dateReceipt,
                                SourceName = pdfName,
                                SourceLine = i,
                                FullData = lines[i].Trim() + " " + lines[i + 1].Trim()
                            };

                            // Lines with *** are for cancellation, don't add
                            if (!name.Contains("***"))
                            {
                                Products.Add(product);
                            }

                            // Skip the second line of the product.
                            i++;
                        }
                    }
                    // Case C: no price on first line
                    else
                    {
                        // Price should be on second with a " x "
                        if (lines[i + 1].Contains(" x "))
                        {
                            var product = new Product()
                            {
                                Name = ExtractProductName(lines[i].Trim()),
                                Price = ExtractProductPrice(lines[i + 1].Trim(), false),
                                Group = productsGroup,
                                DateReceipt = dateReceipt,
                                SourceName = pdfName,
                                SourceLine = i,
                                FullData = lines[i].Trim() + " " + lines[i + 1].Trim()
                            };

                            // Lines with *** are for cancellation, don't add
                            if (!product.Name.Contains("***"))
                            {
                                Products.Add(product);
                            }

                            // Skip the second line of the product.
                            i++;
                        }
                    }

                    // Search the products end
                    if (lines[i].Contains(_oldEndMarker))
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Extract the date for the array lines at position i + 1
        /// </summary>
        /// <param name="lines">array of string</param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static DateTime ExtractDate(string[] lines, int i)
        {
            var dateReceipt = new DateTime();
            var match = RegexDate().Match(lines[i + 1]);
            if (match.Success)
            {
                dateReceipt = DateTime.ParseExact(match.ToString(), "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
            }

            return dateReceipt;
        }

        /// <summary>
        /// Extract products from lines in Products, new format version
        /// </summary>
        /// <param name="pdfName">name of the pdf</param>
        /// <param name="lines">array of lines</param>
        /// <param name="markerIndex">index after marker where products starts</param>
        private void NewFormat(string pdfName, string[] lines, int markerIndex)
        {
            var dateReceipt = new DateTime();
            var productsFound = false;
            var productsGroup = "";

            int dateIndex = Array.FindIndex(lines, l => l.StartsWith("Date"));

            if (dateIndex != -1)
            {
                dateReceipt = ExtractDate(lines, dateIndex);
            }

            for (var i = markerIndex + 1; i < lines.Length; i++)
            {
                // Search the products end
                if (lines[i].Contains(_newEndMarker))
                {
                    break;
                }

                // No €, must be a group name
                if (!lines[i].Contains('€'))
                {
                    productsFound = true;
                    productsGroup = lines[i].Trim();
                }
                else if (productsFound)
                {
                    /*
                     *  Different cases
                     *  A) group with one product, weight on second line
                     *  CONFITURES MIEL P.A.TARTINER
                     *  CONFITURE EXTRA RHUBARBE U370G (T) 1,81 € 11
                     *  1 x 1,81 EUR
                     *  B) group with one product, weight per kg on third line
                     *  FRUITS
                     *  POMME REINETTE GRISE DU CANADA (T) 2,69 € 11
                     *  0,898 kg x 2,99 €/kg
                     *  C) group with multiple products
                     *  PATIS.INDUSTRIELLE
                     *  4/4 PUR BEURRE U 800G (T) 4,54 € 11
                     *  1 x 4,54 EUR
                     *  PANET.NOCCIOLA P.MOTTA 750G (T) 10,40 € 11
                     *  1 x 10,40 EUR
                     *  D) group with volume on second line and
                     *  LAITS ET DERIVES
                     *  LAIT 1/2 EC.PPX BRIQUE 1L (T) 2,82 € 11
                     *  3 x 0,94 EUR
                     *
                     *  group name have no number ?
                     *  product list end with "Nombre de lignes d'article \d+"
                     */
                    var name = ExtractProductName(lines[i].Trim());
                    var product = new Product()
                    {
                        Name = name,
                        Price = ExtractProductPrice(lines[i + 1].Trim(), false),
                        Group = productsGroup,
                        DateReceipt = dateReceipt,
                        SourceName = pdfName,
                        SourceLine = i,
                        FullData = lines[i].Trim() + " " + lines[i + 1].Trim()
                    };

                    // Lines with *** are for cancellation, don't add
                    if (!name.Contains("***"))
                    {
                        Products.Add(product);
                    }

                    // Skip the second line of the product.
                    i++;
                }
            }
        }

        /// <summary>
        /// Extract product name from line.
        /// </summary>
        /// <param name="line">line with product name</param>
        /// <returns></returns>
        public static string ExtractProductName(string line)
        {
            /*
             * Differents cases:
             *
             * LAIT 1/2 EC.PPX BRIQUE 1L        (T)        0,78 €  11
             *
             * LAIT 1/2 EC.PPX BRIQUE 1L            (T)
             *
             * MAD.COQ.OEUF PA.ST MICHEL X24               3,59 €  11
             *
             * BANANE CAVENDISH SCB PREMIUM                        11
             *
             * LAIT 1/2 EC.PPX BRIQUE 1L
             *
             */
            //Product end at position 33 (where the (T) appears).
            return line[..int.Min(line.Length, 33)].Trim();
        }

        /// <summary>
        /// Extract product price from line.
        /// </summary>
        /// <param name="line">line with price</param>
        /// <param name="priceAtEnd">true if price is at end</param>
        /// <returns></returns>
        public static decimal ExtractProductPrice(string line, bool priceAtEnd)
        {
            Regex regexStart;
            Regex regexEnd;
            if (line.Contains('€'))
            {
                regexStart = RegexPriceAtStart();
                regexEnd = RegExPriceAtEnd();
            }
            else
            {
                regexStart = RegexPriceAtStartEUR();
                regexEnd = RegExPriceAtEndEUR();
            }
            /* Differents cases:
             *
             * POMME GOLDEN DELICIOUS                      1,51 €  11
             *
             * 0,898 kg x     2,69 €/ kg         2,42 €
             *
             * 2 x     0,74 €                          1,48 €  11
             */
            // No " x ", price at the end.
            if (priceAtEnd)
            {
                var match = regexEnd.Match(line);
                if (match.Success)
                {
                    return decimal.Parse(match.Groups[1].Value) + decimal.Parse(match.Groups[2].Value) / 100m;
                }
            }
            // Take the Price by kg/L.
            else
            {
                var match = regexStart.Match(line);
                if (match.Success)
                {
                    return decimal.Parse(match.Groups[1].Value) + decimal.Parse(match.Groups[2].Value) / 100m;
                }
            }

            return 0;
        }

        // A string finishing by : a decimal, one or more spaces, the symbol € and maybe (one or more spaces and a number).
        [GeneratedRegex("(\\d+),(\\d\\d) +€( +\\d+)?$")]
        private static partial Regex RegExPriceAtEnd();

        // A string finishing by : a decimal, one or more spaces, the symbol EUR and maybe (one or more spaces and a number).
        [GeneratedRegex("(\\d+),(\\d\\d) EUR( +\\d+)?$")]
        private static partial Regex RegExPriceAtEndEUR();

        // A string starting by : anything, " x" followed by decimal, the symbol €.
        [GeneratedRegex("^.* +x +(\\d+),(\\d\\d) +€")]
        private static partial Regex RegexPriceAtStart();

        // A string starting by : anything, " x" followed by decimal, the symbol EUR.
        [GeneratedRegex("^.* +x +(\\d+),(\\d\\d) +EUR")]
        private static partial Regex RegexPriceAtStartEUR();

        // Short date.
        [GeneratedRegex("([0-9][0-9])/([0-9][0-9])/([0-9][0-9])")]
        private static partial Regex RegexDate();
    }
}