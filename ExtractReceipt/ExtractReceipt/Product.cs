namespace ExtractReceipt
{
    public class Product
    {
        //Key for db.
        public int Id { get; set; }

        //Name of the product.
        public string? Name { get; set; }

        //Group of the product.
        public string? Group { get; set; }

        //Price of the product or by kg/L if multiple products.
        public decimal Price { get; set; }

        //Date of the receipt
        public DateTime DateReceipt { get; set; }

        //File name of the receipt.
        public string? SourceName { get; set; }

        //Line ine the receipt.
        public int SourceLine { get; set; }

        //Full data of the product for debug.
        public string? FullData { get; set; }

        public override string ToString() => $"{DateReceipt:yyyy-MM-dd};{Group};{Name};{Price};{SourceName};{SourceLine};{FullData}";
    }
}