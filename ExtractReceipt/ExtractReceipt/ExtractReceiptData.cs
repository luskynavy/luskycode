using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ExtractReceipt
{
    internal partial class ExtractReceiptData
    {
        //List of products.
        public List<Product>? Products { get; set; }

        /// <summary>
        /// Extract products list from the text.
        /// </summary>
        /// <param name="pdfName">source file name</param>
        /// <param name="text">text with all data</param>
        public void ExtractData(string pdfName, string text)
        {
            Products = new List<Product>();

            var dateReceipt = new DateTime();

            var lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            if (lines != null)
            {
                var productsFound = false;
                var productsGroup = "";
                for (var i = 0; i < lines.Length; i++)
                {
                    //Search the receipt date.
                    if (lines[i].Contains("Date"))
                    {
                        //Next line contain the date.
                        var match = RegexDate().Match(lines[i + 1]);
                        if (match.Success)
                        {
                            dateReceipt = DateTime.ParseExact(match.ToString(), "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //Skip the line with date.
                        i++;
                    }

                    //Search the products start.
                    if (lines[i].Trim().StartsWith(">>>>"))
                    {
                        productsFound = true;
                        productsGroup = lines[i].Trim()[4..].Trim();
                    }
                    else if (productsFound)
                    {
                        /*Differents cases
                         *A) A group with one product, one line with price:
                         * >>>> LAITS ET DERIVES
                         * LAIT 1/2 EC.PPX BRIQUE 1L        (T)        0,78 €  11
                         * >>>> TRAITEUR LS UVCI
                         *
                         *A group with different products, one line by product with its price:
                         * >>>> FROMAGE LS
                         * REBLOC AOP CRU 27% POCHAT450G               5,82 €  11
                         * EMM.PAST.28%MG U PORTION 400G               3,09 €  11
                         * >>>> TRAITEUR LS UVCI
                         *
                         *B) A product on two lines, price on first line, " x " on second line, price by kg/L on second line:
                         * >>>> FRUITS
                         * POMME GOLDEN DELICIOUS                      1,51 €  11
                         * 0,892 kg  x     1,69 €/kg
                         * >>>> PATIS.INDUSTRIELLE
                         *
                         *C) A product on two lines, no price on first line, " x " on second line, price on second line:
                         * >>>> LAITS ET DERIVES
                         * LAIT 1/2 EC.PPX BRIQUE 1L            (T)
                         * 2 x     0,74 €                          1,48 €  11
                         * >>>> SURGELE SALE
                         *
                         *A product on two lines, no price on first line, " x " on second line, price on second line:
                         * >>>> FRUITS
                         * POMME GOLDEN DELICIOUS PILAT      (T)               11
                         * 0,898 kg  x     2,69 €/kg         2,42 €
                         * >>>> SURGELE SALE
                         *
                         *A product on two lines, no price on first line, " x " on second line, price on second line:
                         * >>>> LAITS ET DERIVES
                         * LAIT 1/2 EC.PPX BRIQUE 1L
                         * 2 x     0,74 €                          1,48 €  11
                         * >>>> FROMAGE LS
                         *
                         */

                        //Price on 1st line: : case A or B.
                        if (lines[i].Contains('€'))
                        {
                            var name = ExtractProductName(lines[i].Trim());
                            //Case A : one product with its price on one line.
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

                                //Lines with *** are for cancellation, don't add
                                if (!name.Contains("***"))
                                {
                                    Products.Add(product);
                                }
                            }
                            //Case B: second line with price by kg/L/product.
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

                                //Lines with *** are for cancellation, don't add
                                if (!name.Contains("***"))
                                {
                                    Products.Add(product);
                                }

                                //Skip the second line of the product.
                                i++;
                            }
                        }
                        //Case C: no price on first line
                        else
                        {
                            //Price should be on second with a " x "
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

                                //Lines with *** are for cancellation, don't add
                                if (!product.Name.Contains("***"))
                                {
                                    Products.Add(product);
                                }

                                //Skip the second line of the product.
                                i++;
                            }
                        }

                        //Search the products end
                        if (lines[i].Contains("==="))
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extract product name from line.
        /// </summary>
        /// <param name="line">line with product name</param>
        /// <returns></returns>
        private static string ExtractProductName(string line)
        {
            /* Differents cases:
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
        /// <param name="priceAtEnd"></param>
        /// <returns></returns>
        private static decimal ExtractProductPrice(string line, bool priceAtEnd)
        {
            /* Differents cases:
             *
             * POMME GOLDEN DELICIOUS                      1,51 €  11
             *
             * 0,898 kg x     2,69 €/ kg         2,42 €
             *
             * 2 x     0,74 €                          1,48 €  11
             */
            //No " x ", price at the end.
            if (priceAtEnd)
            {
                var match = RegExPriceAtEnd().Match(line);
                if (match.Success)
                {
                    return decimal.Parse(match.Groups[1].Value) + decimal.Parse(match.Groups[2].Value) / 100m;
                }
            }
            //Take the Price by kg/L.
            else
            {
                var match = RegexPriceAtStart().Match(line);
                if (match.Success)
                {
                    return decimal.Parse(match.Groups[1].Value) + decimal.Parse(match.Groups[2].Value) / 100m;
                }
            }

            return 0;
        }

        //A string finishing by : a decimal, one or more spaces, the symbol € and maybe (one or more spaces and a number).
        [GeneratedRegex("(\\d+),(\\d\\d) +€( +\\d+)?$")]
        private static partial Regex RegExPriceAtEnd();

        // A string starting by : anything, " x" followed by decimal, the symbol €.
        [GeneratedRegex("^.* +x +(\\d+),(\\d\\d) +€")]
        private static partial Regex RegexPriceAtStart();

        //Short date.
        [GeneratedRegex("([0-9][0-9])/([0-9][0-9])/([0-9][0-9])")]
        private static partial Regex RegexDate();
    }
}