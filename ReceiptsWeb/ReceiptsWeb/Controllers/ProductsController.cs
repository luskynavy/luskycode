using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using MiniExcelLibs;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using ReceiptsWeb.Models;

namespace ReceiptsWeb.Controllers
{
	public class ProductsController : Controller
	{
		private const string _cookieDefaultSortName = "DefaultSort";
		private const string _cookieDefaultGroupSortName = "DefaultGroupSort";
		private readonly ReceiptsContext _context;
		private readonly IStringLocalizer<ProductsController> _localizer;
		private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
		private readonly int pageSizeDefault = 20;

		public ProductsController(ReceiptsContext context, IStringLocalizer<ProductsController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
		{
			_context = context;
			_localizer = localizer;
			_sharedLocalizer = sharedLocalizer;
		}

		// GET: Products
		public async Task<IActionResult> Index(string searchString, string filterGroup, string sort, string pageSize, int? pageNumber)
		{
			int pageSizeInt = pageSizeDefault;

			if (!pageSize.IsNullOrEmpty())
			{
				pageSizeInt = int.Parse(pageSize);
			}
			else
			{
				pageSize = pageSizeDefault.ToString();
			}

			//Default sort from cookies if sort is null
			if (sort.IsNullOrEmpty())
			{
				var defaultSort = Request.Cookies.FirstOrDefault(c => c.Key.Equals(_cookieDefaultSortName));
				sort = defaultSort.Value;
			}
			else
			{
				Response.Cookies.Append(_cookieDefaultSortName,
				sort,
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
			}

			//Values for view
			ViewBag.searchString = searchString;
			ViewBag.filterGroup = filterGroup;
			ViewBag.sort = sort;
			ViewBag.pageSize = pageSize;

			//Select lists
			ViewBag.GroupList = GroupSelectList(filterGroup);
			ViewBag.ProductsSortList = ProductsSortList(sort);
			ViewBag.PageSizeList = PageSizeList(pageSize);

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
			if (sort == "DateReceipt")
			{
				products = products.OrderByDescending(p => p.DateReceipt);
			}
			else if (sort == "Name")
			{
				products = products.OrderBy(p => p.Name);
			}
			else //Group or unknown
			{
				products = products.OrderBy(p => p.Group).ThenBy(p => p.Name).ThenBy(p => p.DateReceipt);
			}

			return products != null ?
				View(await PaginatedList<Products>.CreateAsync(products, pageNumber ?? 1, pageSizeInt)) :
				Problem("Entity set 'ReceiptsContext.Products'  is null.");
		}

		// GET: GroupProducts
		public async Task<IActionResult> GroupProducts(string searchString, string filterGroup, string sort, string pageSize, int? pageNumber)
		{
			int pageSizeInt = pageSizeDefault;

			if (!pageSize.IsNullOrEmpty())
			{
				pageSizeInt = int.Parse(pageSize);
			}
			else
			{
				pageSize = pageSizeDefault.ToString();
			}

			//Default sort from cookies if sort is null
			if (sort.IsNullOrEmpty())
			{
				var defaultSort = Request.Cookies.FirstOrDefault(c => c.Key.Equals(_cookieDefaultGroupSortName));
				sort = defaultSort.Value;
			}
			else
			{
				Response.Cookies.Append(_cookieDefaultGroupSortName,
				sort,
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
			}

			//Values for view
			ViewBag.searchString = searchString;
			ViewBag.filterGroup = filterGroup;
			ViewBag.sort = sort;
			ViewBag.pageSize = pageSize;

			//Select lists
			ViewBag.GroupList = GroupSelectList(filterGroup);
			ViewBag.GroupProductsSortList = GroupProductsSortList(sort);
			ViewBag.PageSizeList = PageSizeList(pageSize);

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
				//Query syntax
				/*var groupsProducts = from p in products
                                     group p by new
                                     {
                                         p.Group,
                                         p.Name
                                     } into gp
                                     select new GroupProducts
                                     {
                                         Id = gp.Min(p => p.Id),
                                         Group = gp.Key.Group,
                                         Name = gp.Key.Name,
                                         Min = gp.Min(x => x.Price),
                                         Max = gp.Max(x => x.Price),
                                         MinDate = gp.Min(p => p.DateReceipt),
                                         MaxDate = gp.Max(p => p.DateReceipt)
                                     };*/

				//Sort
				if (sort == "PriceRatio")
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
				else //Group or unknown
				{
					groupsProducts = groupsProducts.OrderBy(p => p.Group).ThenBy(p => p.Name);
				}

				return View(await PaginatedList<GroupProducts>.CreateAsync(groupsProducts, pageNumber ?? 1, pageSizeInt));
			}
			else
			{
				return Problem("Entity set 'ReceiptsContext.Products'  is null.");
			}
		}

		/// <summary>
		/// Create group select list
		/// </summary>
		/// <param name="filterGroup">selected value</param>
		/// <returns></returns>
		private List<SelectListItem> GroupSelectList(string filterGroup)
		{
			var groupList = _context.Products.Select(p => p.Group).Distinct().ToList();
			var selectList = new List<SelectListItem>
			{
				new SelectListItem { Text = "", Value = "" }
			};
			foreach (var group in groupList)
			{
				if (filterGroup == group)
				{
					selectList.Add(new SelectListItem { Text = group, Value = group, Selected = true });
				}
				else
				{
					selectList.Add(new SelectListItem { Text = group, Value = group });
				}
			};
			return selectList;
		}

		/// <summary>
		/// Create products select list for sort
		/// </summary>
		/// <param name="value">selected value</param>
		/// <returns></returns>
		private List<SelectListItem> ProductsSortList(string value)
		{
			var selectList = new List<SelectListItem>
			{
				new SelectListItem { Text = _sharedLocalizer["Group"], Value = "Group"},
				new SelectListItem { Text = _sharedLocalizer["DateReceipt"], Value = "DateReceipt"},
				new SelectListItem { Text = _sharedLocalizer["Name"], Value = "Name"}
			};

			var selected = selectList.Find(p => p.Value == value);
			if (selected != null)
			{
				selected.Selected = true;
			}

			return selectList;
		}

		/// <summary>
		/// Create group products select list for sort
		/// </summary>
		/// <param name="value">selected value</param>
		/// <returns></returns>
		private List<SelectListItem> GroupProductsSortList(string value)
		{
			var selectList = new List<SelectListItem>
			{
				new SelectListItem { Text = _sharedLocalizer["Group"], Value = "Group"},
				new SelectListItem { Text = _sharedLocalizer["PriceRatio"], Value = "PriceRatio" },
				new SelectListItem { Text = _sharedLocalizer["PricesCount"], Value = "PricesCount" },
				new SelectListItem { Text = _sharedLocalizer["MaxDate"], Value = "MaxDate" },
			};

			var selected = selectList.Find(p => p.Value == value);
			if (selected != null)
			{
				selected.Selected = true;
			}

			return selectList;
		}

		/// <summary>
		/// Create select list for page size
		/// </summary>
		/// <param name="value">selected value</param>
		/// <returns></returns>
		private List<SelectListItem> PageSizeList(string value)
		{
			var selectList = new List<SelectListItem>
			{
				new SelectListItem { Text = "10", Value = "10"},
				new SelectListItem { Text = "20", Value = "20"},
				new SelectListItem { Text = "100", Value = "100" },
				new SelectListItem { Text = _sharedLocalizer["All"], Value = "100000" }
			};

			var selected = selectList.Find(p => p.Value == value);
			if (selected != null)
			{
				selected.Selected = true;
			}

			return selectList;
		}

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var products = await _context.Products
				.FirstOrDefaultAsync(m => m.Id == id);
			if (products == null)
			{
				return NotFound();
			}

			return View(products);
		}

		// GET: Products/GetProductPrices/5
		public ActionResult GetProductPrices(int id)
		{
			//return ViewComponent("ProductPrices", new { id });
			return ViewComponent(typeof(ProductPricesViewComponent), new { id });
		}

		// GET: Products/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Group,Price,DateReceipt,SourceName,SourceLine,FullData")] Products products)
		{
			if (ModelState.IsValid)
			{
				_context.Add(products);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(products);
		}

		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var products = await _context.Products.FindAsync(id);
			if (products == null)
			{
				return NotFound();
			}
			return View(products);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Group,Price,DateReceipt,SourceName,SourceLine,FullData")] Products products)
		{
			if (id != products.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(products);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductsExists(products.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(products);
		}

		// GET: Products/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Products == null)
			{
				return NotFound();
			}

			var products = await _context.Products
				.FirstOrDefaultAsync(m => m.Id == id);
			if (products == null)
			{
				return NotFound();
			}

			return View(products);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Products == null)
			{
				return Problem("Entity set 'ReceiptsContext.Products'  is null.");
			}
			var products = await _context.Products.FindAsync(id);
			if (products != null)
			{
				_context.Products.Remove(products);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductsExists(int id)
		{
			return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
		}

		public IActionResult LiveTagSearch(string search)
		{
			// Call your method to search your data source here.
			// I'll use entity framework to query my DB
			var res = (
				from t in _context.Products
				where t.Name.Contains(search)
				select new Tag { Id = t.Id, Name = t.Name }
				).Take(10).ToList();

			// Pass the List of results to a Partial View
			return PartialView(res);
		}

		public JsonResult LiveTagSearchJson(string search, string group)
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

			var res = products.Select(p => new { p.Name })
				.Distinct().Take(10);

			return Json(res);
		}

		public IActionResult ExportMiniExcel()
		{
			IQueryable<Products> products = _context.Products;

			var memoryStream = new MemoryStream();
			//MiniExcel SaveAs extension
			memoryStream.SaveAs(products);
			memoryStream.Seek(0, SeekOrigin.Begin);
			return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				//FileDownloadName = "MiniExcel.xlsx"
			};
		}

		public IActionResult ExportEPPlus()
		{
			IQueryable<Products> products = _context.Products;

			//Needed for version 5 or above
			//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var ep = new ExcelPackage())
			{
				ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Products");
				ws.Cells["A1"].LoadFromCollection(products.ToList(), true);
				return new FileContentResult(ep.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
				{
					FileDownloadName = "EPPlus.xlsx"
				};
			}
		}
	}
}