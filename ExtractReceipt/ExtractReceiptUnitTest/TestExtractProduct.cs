using ExtractReceipt;
using static System.Net.Mime.MediaTypeNames;

namespace ExtractReceiptUnitTest
{
    [TestClass]
    public sealed class TestExtractProduct
    {
        [TestMethod]
        [DataRow("LAIT 1/2 EC.PPX BRIQUE 1L        (T)        0,78 €  11", "LAIT 1/2 EC.PPX BRIQUE 1L")]
        [DataRow("LAIT 1/2 EC.PPX BRIQUE 1L            (T)", "LAIT 1/2 EC.PPX BRIQUE 1L")]
        [DataRow("MAD.COQ.OEUF PA.ST MICHEL X24               3,59 €  11", "MAD.COQ.OEUF PA.ST MICHEL X24")]
        [DataRow("BANANE CAVENDISH SCB PREMIUM                        11", "BANANE CAVENDISH SCB PREMIUM")]
        [DataRow("LAIT 1/2 EC.PPX BRIQUE 1L", "LAIT 1/2 EC.PPX BRIQUE 1L")]
        public void TestExtractProductName(string line, string expected)
        {
            //Act
            var res = ExtractReceiptData.ExtractProductName(line);

            //Assert
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        [DataRow("LAIT 1/2 EC.PPX BRIQUE 1L        (T)        0,78 €  11", true, 0.78)]
        [DataRow("LAIT 1/2 EC.PPX BRIQUE 1L            (T)", true, 0)]
        [DataRow("MAD.COQ.OEUF PA.ST MICHEL X24               3,59 €  11", true, 3.59)]
        [DataRow("MAD.COQ.OEUF PA.ST MICHEL X24               13,59 €  11", true, 13.59)]
        [DataRow("BANANE CAVENDISH SCB PREMIUM                        11", true, 0)]
        [DataRow("LAIT 1/2 EC.PPX BRIQUE 1L", true, 0)]
        [DataRow("POMME GOLDEN DELICIOUS                      1,51 €  11", true, 1.51)]
        [DataRow("POMME GOLDEN DELICIOUS                      21,51 €  11", true, 21.51)]
        [DataRow("0,898 kg x     2,69 €/ kg         2,42 €", false, 2.69)]
        [DataRow("0,898 kg x     32,69 €/ kg         2,42 €", false, 32.69)]
        [DataRow("2 x     0,74 €                          1,48 €  11", false, 0.74)]
        [DataRow("2 x     100,74 €                          1,48 €  11", false, 100.74)]
        public void TestExtractProductPrice(string line, bool priceAtEnd, double expected)
        {
            //Act
            var res = ExtractReceiptData.ExtractProductPrice(line, priceAtEnd);

            //Assert
            Assert.AreEqual((decimal)expected, res);
        }
    }
}
