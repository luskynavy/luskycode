using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReceiptsWeb.Models;
using System.Linq;
using webapi.Models;

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
		public IEnumerable<Products> Get(string? filterGroup, string? searchString, string? sort, string? pageSize, int? pageNumber)
		{
			int pageSizeInt = pageSizeDefault;

			if (!pageSize.IsNullOrEmpty())
			{
				 int.TryParse(pageSize, out pageSizeInt);
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

			products = products.Take(pageSizeInt);

			var res = products.ToList();
			return res;
		}

		// GET /<ProductsController>/5
		[HttpGet("{id}")]
		public Products Get(int id)
		{
			var products = _context.Products.First(p => p.Id == id);
			var res = products;
			return res;
		}

		// GET: /<ProductsController>
		[HttpGet]
		[Route("~/GroupProducts")]
		public IEnumerable<GroupProducts> GroupProducts()
		{
			var groupsProducts = _context.Products.GroupBy(
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

			groupsProducts = groupsProducts.Take(10);
			var res = groupsProducts.ToList();
			return res;
		}

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
	}
}