﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReceiptsWeb.Models;
using webapi.Models;
using MiniExcelLibs;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ReceiptsContext _context;
		private readonly int pageSizeDefault = 10;

		public ProductsController(ReceiptsContext context)
		{
			_context = context;
		}

		// GET: /<ProductsController>
		[HttpGet]
		public PaginatedList<ProductExt> Get(string? filterGroup, string? searchString, string? sort, string? pageSize, int? pageNumber)
		{
			int pageSizeInt = pageSizeDefault;

			if (!pageSize.IsNullOrEmpty())
			{
				if (int.TryParse(pageSize, out var i))
				{
					pageSizeInt = i;
				}
			}
			else
			{
				pageSize = pageSizeDefault.ToString();
			}

			IQueryable<Products> products = _context.Products;

			//Filter
			if (!String.IsNullOrEmpty(filterGroup))
			{
				products = products.Where(s => s.Group.Equals(filterGroup));
			}

			if (!String.IsNullOrEmpty(searchString))
			{
				products = products.Where(s => s.Name.Contains(searchString));
			}

			//Sort
			if (sort == "Group" || sort.IsNullOrEmpty())
			{
				products = products.OrderBy(p => p.Group).ThenBy(p => p.Name).ThenBy(p => p.DateReceipt);
			}
			else if (sort == "DateReceipt")
			{
				products = products.OrderByDescending(p => p.DateReceipt);
			}
			else if (sort == "Name")
			{
				products = products.OrderBy(p => p.Name);
			}

			products = products.Select(p => new Products
			{
				Id = p.Id,
				Name = p.Name,
				DateReceipt = p.DateReceipt,
				FullData = p.FullData,
				Group = p.Group,
				Price = p.Price,
				SourceLine = p.SourceLine,
				SourceName = System.IO.Path.GetFileName(p.SourceName)
			});

			var res = PaginatedList<Products>.Create(products, pageNumber ?? 1, pageSizeInt);

			var productExtList = new List<ProductExt>();
            //Convert Product in Products and extract price per kilo
            foreach (var p in res.Data)
            {
                var p2 = new ProductExt
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateReceipt = p.DateReceipt,
                    FullData = p.FullData,
                    Group = p.Group,
                    Price = p.Price,
                    SourceLine = p.SourceLine,
                    SourceName = p.SourceName,
                    PricePerKilo = Math.Round(ExtractPricePerKilo(p.Name, p.Price), 2)
                };
                productExtList.Add(p2);
            }

            var paginatedListProductExt = new PaginatedList<ProductExt>(productExtList, res.PageIndex, res.TotalPages);

            return paginatedListProductExt;
		}

		// GET /<ProductsController>/5
		[HttpGet("{id}")]
		public Products Get(int id)
		{
			var products = _context.Products.First(p => p.Id == id);
			var res = products;
			return res;
		}

		// GET: /<GroupProducts>
		[HttpGet]
		[Route("~/GroupProducts")]
		public PaginatedList<GroupProducts> GroupProducts(string? filterGroup, string? searchString, string? sort, string? pageSize, string? products1price, int? pageNumber)
		{
			int pageSizeInt = pageSizeDefault;

			if (!pageSize.IsNullOrEmpty())
			{
				if (int.TryParse(pageSize, out var i))
				{
					pageSizeInt = i;
				}
			}
			else
			{
				pageSize = pageSizeDefault.ToString();
			}

			IQueryable<Products> products = _context.Products;

			//Filter
			if (!String.IsNullOrEmpty(filterGroup))
			{
				products = products.Where(s => s.Group.Equals(filterGroup));
			}

			if (!String.IsNullOrEmpty(searchString))
			{
				products = products.Where(s => s.Name.Contains(searchString));
			}

			if (products != null)
			{
				//Method syntax
				var groupsProducts = products.GroupBy(
										 p => new
										 {
											 p.Group,
											 p.Name
										 }).
										Select(gp => new GroupProducts
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
				//Filter products with only one price (useless for comparaison)
				if (products1price != "true")
				{
					groupsProducts = groupsProducts.Where(p => p.PricesCount > 1);
				}

				//Sort
				if (sort == "Group" || sort.IsNullOrEmpty())
				{
					groupsProducts = groupsProducts.OrderBy(p => p.Group).ThenBy(p => p.Name);
				}
				else if (sort == "PriceRatio")
				{
					groupsProducts = groupsProducts.OrderByDescending(p => p.PriceRatio);
				}
				else if (sort == "PricesCount")
				{
					groupsProducts = groupsProducts.OrderByDescending(p => p.PricesCount);
				}
				else if (sort == "MaxDate")
				{
					groupsProducts = groupsProducts.OrderByDescending(p => p.MaxDate);
				}

				var res = PaginatedList<GroupProducts>.Create(groupsProducts, pageNumber ?? 1, pageSizeInt);

                //Get the previous different price
                foreach (var p in res.Data)
                {
                    //Get the prices list for the product
                    var prices = p.PricesList;
                    //Skip the prices if they are equal to older
                    p.PreviousPrice = prices.SkipWhile((z, index) => z == p.LastPrice && index != prices.Count() - 1).First();

                    p.LastPricePerKilo = Math.Round(ExtractPricePerKilo(p.Name, p.LastPrice), 2);
                }
                return res;
			}
			else
			{
				return new PaginatedList<GroupProducts>(new List<GroupProducts>(), 0, 1, pageSizeInt);
			}
		}

		/// <summary>
		/// Create group select list
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("~/GroupSelectList")]
		public List<string> GroupSelectList()
		{
			var groupList = _context.Products.Select(p => p.Group).Distinct().ToList();
			var selectList = new List<string>();
			selectList.Add("");
			foreach (var group in groupList)
			{
				selectList.Add(group);
			};
			return selectList;
		}

		[HttpGet]
		[Route("~/GetProductPrices")]
		public List<ProductsPrices> GetProductPrices(int id)
		{
			if (_context.Products == null)
			{
				return new List<ProductsPrices>();
			}

			var product = _context.Products.FirstOrDefault(m => m.Id == id);

			if (product != null)
			{
				var products = _context.Products.Where(m => m.Name == product.Name)
					.Select(gp => new ProductsPrices
					{
						Price = gp.Price,
						DateReceipt = gp.DateReceipt
					}).OrderBy(p => p.DateReceipt);

				return products.ToList();
			}

			return new List<ProductsPrices>();
		}

		[HttpGet]
		[Route("~/ProductsNames")]
		public List<string> ProductsNames(string? search, string? group)
		{
			IQueryable<Products> products = _context.Products;

			if (search != null)
			{
				products = products.Where(p => p.Name.Contains(search));
			}

			if (group != null)
			{
				products = products.Where(p => p.Group.Equals(group));
			}

			var res = products.Select(p => p.Name)
				.Distinct();

			return res.ToList();
		}

		[HttpGet]
		[Route("~/ExportProductsMiniExcel")]
		public IActionResult ExportProductsMiniExcel()
		{
			IQueryable<Products> products = _context.Products;

			var memoryStream = new MemoryStream();
			//MiniExcel SaveAs extension
			memoryStream.SaveAs(products);
			memoryStream.Seek(0, SeekOrigin.Begin);
			return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileDownloadName = "MiniExcel.xlsx"
			};
		}

		[HttpGet]
		[Route("~/ExportGroupProductsMiniExcel")]
		public IActionResult ExportGroupProductsMiniExcel()
		{
			IQueryable<Products> products = _context.Products;

			var groupsProducts = products.GroupBy(
									 p => new
									 {
										 p.Group,
										 p.Name
									 }).
									Select(gp => new GroupProducts
									{
										Id = gp.Min(p => p.Id),
										Group = gp.Key.Group,
										Name = gp.Key.Name,
										Min = gp.Min(p => p.Price),
										Max = gp.Max(p => p.Price),
										MinDate = gp.Min(p => p.DateReceipt),
										MaxDate = gp.Max(p => p.DateReceipt),
										PriceRatio = gp.Max(p => p.Price) / gp.Min(p => p.Price),
										PricesCount = gp.Count()
									});

			var memoryStream = new MemoryStream();
			//MiniExcel SaveAs extension
			memoryStream.SaveAs(groupsProducts);
			memoryStream.Seek(0, SeekOrigin.Begin);
			return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileDownloadName = "MiniExcel.xlsx"
			};
		}

        /// <summary>
        /// Extract the price per kilo from product name if possible
        /// </summary>
        /// <param name="name">product name with price</param>
        /// <param name="price">price of product</param>
        /// <returns></returns>
        private static decimal ExtractPricePerKilo(string name, decimal price)
        {
            //Search weight in gramme (G or GR)
            // ' 400G' or '.400G' or ' 400GR'
            string pattern = @"( |\.)(\d+)(G|GR)$";
            var match = Regex.Match(name, pattern);

            if (match.Success)
            {
                return price / (decimal.Parse(match.Groups[2].Value) / 1000);
            }
            else
            {
                //Search weight in gramme (G or GR) with multiple products
                // ' 2X100G' or '.2X100G' or ' 2X100GR'
                pattern = @"( |\.)(\d+)X(\d+)(G|GR)$";
                match = Regex.Match(name, pattern);

                if (match.Success)
                {
                    return price / (decimal.Parse(match.Groups[2].Value) * decimal.Parse(match.Groups[3].Value) / 1000);
                }
                else
                {
                    //Search weight in kilogramme in integer format
                    // ' 1KG' or '.2KG'
                    pattern = @"( |\.)(\d+)KG$";
                    match = Regex.Match(name, pattern);

                    if (match.Success)
                    {
                        return price / (decimal.Parse(match.Groups[2].Value));
                    }
                    else
                    {
                        //Search weight in kilogramme in decimal format
                        // ' 1,5KG' or '.1,5KG'
                        pattern = @"( |\.)(\d+),(\d+)KG$";
                        match = Regex.Match(name, pattern);

                        if (match.Success)
                        {
                            var weightIntegerPart = decimal.Parse(match.Groups[2].Value);
                            var weightDecimalPart = decimal.Parse(match.Groups[3].Value) / (decimal)Math.Pow(10, match.Groups[3].Value.ToString().Length);
                            return price / (weightIntegerPart + weightDecimalPart);
                        }
                    }
                }
            }

            return 0;
        }

        /*
		// POST /<ProductsController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT /<ProductsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE /<ProductsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
		*/
    }
}