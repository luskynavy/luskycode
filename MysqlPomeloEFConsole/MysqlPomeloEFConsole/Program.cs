using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MysqlPomeloEFConsole.Models;
using System.Linq;

namespace MysqlPomeloEFConsole
{
    internal class Program
    {
        static void TestEuro(MysqlDbContext context)
        {
            var rand = new Random();
            var foreuro = new Foreuro()
            {
                MyEuroColumn = rand.Next(1, 100).ToString() + "€10"
            };

            //Add ok avec mysql 8
            //Add ok avec mariadb 11.7.2
            context.Foreuros.Add(foreuro);
            context.SaveChanges();

            var euro = context.Foreuros.OrderBy(p => p.Id).LastOrDefault();

            if (euro != null)
            {
                Console.WriteLine($"Id: {euro.Id}");
                Console.WriteLine($"MyEuroColumn: {euro.MyEuroColumn}");
            }
        }

        private static void TestProduct(MysqlDbContext context)
        {
            var rand = new Random();
            int price = rand.Next(1, 100);
            var date = DateTime.Now;

            var product = new Product()
            {
                Group = "fruits",
                Name = "bananes",
                Price = price + .3m,
                DateReceipt = new DateTime(date.Year, date.Month, date.Day),
                SourceName = "test.pdf",
                SourceLine = rand.Next(1, 100),
                FullData = "bananes " + price.ToString() + "€30 11"
            };

            Product? res = ShowLastProduct(context);

            context.Products.Add(product);
            context.SaveChanges();

            Console.WriteLine();

            res = ShowLastProduct(context);
        }

        private static Product? ShowLastProduct(MysqlDbContext context)
        {
            var res = context.Products.OrderBy(p => p.Id).LastOrDefault();

            if (res != null)
            {
                Console.WriteLine($"Id: {res.Id}");
                Console.WriteLine($"Group: {res.Group}");
                Console.WriteLine($"Name: {res.Name}");
                Console.WriteLine($"Price: {res.Price}");
                Console.WriteLine($"DateReceipt: {res.DateReceipt}");
                Console.WriteLine($"SourceName: {res.SourceName}");
                Console.WriteLine($"SourceLine: {res.SourceLine}");
                Console.WriteLine($"FullData: {res.FullData}");
            }

            return res;
        }

        private static void TestGroupProduct(MysqlDbContext context)
        {
            var groupsProducts = context.Products.
                GroupBy(
                    p => new
                    {
                        p.Group,
                        p.Name
                    }).
                Select(gp => new /*GroupProducts*/
                {
                    Id = gp.Max(p => p.Id),
                    Group = gp.Key.Group,
                    Name = gp.Key.Name,
                    Min = gp.Min(p => p.Price),
                    Max = gp.Max(p => p.Price),
                    //if there is at least 2 elements, sort by date, skip the last, so we have the previous product
                    //PreviousPrice = gp.Count() >= 2 ? gp.OrderByDescending(x => x.DateReceipt).Skip(1).First().Price : gp.First().Price,
                    PricesList = gp.OrderByDescending(x => x.DateReceipt).Select(z => z.Price),
                    LastPrice = gp.OrderByDescending(x => x.DateReceipt).First().Price,
                    MinDate = gp.Min(p => p.DateReceipt),
                    MaxDate = gp.Max(p => p.DateReceipt),
                    PriceRatio = gp.Max(p => p.Price) / gp.Min(p => p.Price),
                    PricesCount = gp.Count()
                });

            var res = groupsProducts.Take(10).ToArray();

            foreach (var product in res)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Group: {product.Group}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Min: {product.Min}");
                Console.WriteLine($"Max: {product.Max}");
                Console.WriteLine($"LastPrice: {product.LastPrice}");
                Console.WriteLine($"MinDate: {product.MinDate}");
                Console.WriteLine($"MaxDate: {product.MaxDate}");
                Console.WriteLine($"MinDate: {product.MinDate}");
                Console.WriteLine($"MaxDate: {product.MaxDate}");
                Console.WriteLine($"PriceRatio: {product.PriceRatio}");
                Console.WriteLine($"PricesCount: {product.PricesCount}");

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var context = new MysqlDbContext();
            TestEuro(context);

            Console.WriteLine();

            TestProduct(context);

            TestGroupProduct(context);
        }
    }
}
