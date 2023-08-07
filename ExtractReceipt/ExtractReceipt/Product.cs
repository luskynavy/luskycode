using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractReceipt
{
    public class Product
    {
        public string? Name { get; set; }
        public string? Group { get; set; }
        public decimal Price { get; set; }
        public DateOnly DateReceipt { get; set; }
        public string? SourceName { get; set; }
        public int SourceLine { get; set; }
        public string? FullData { get; set; }
    }
}