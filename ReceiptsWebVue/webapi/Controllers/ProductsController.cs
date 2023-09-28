using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceiptsWeb.Models;
using webapi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ReceiptsContext _context;

		public ProductsController(ReceiptsContext context)
		{
			_context = context;
		}

		// GET: /<ProductsController>
		[HttpGet]
		public IEnumerable<Products> Get()
		{
			var products = _context.Products.Take(10);
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