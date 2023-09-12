using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ReceiptsWeb.Models;

namespace ReceiptsWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ReceiptsContext _context;
        private int pageSizeDefault = 20;

        public ProductsController(ReceiptsContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, string filterGroup, string sort, int? pageNumber)
        {
            ViewData["searchString"] = searchString;
            ViewData["filterGroup"] = filterGroup;
            ViewData["sort"] = sort;

            //Select lists
            ViewBag.GroupList = GroupSelectList(filterGroup);
            ViewBag.ProductsSortList = ProductsSortList(sort);

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
            if (sort == "Group")
            {
                products = products/*.Take(20)*/.OrderBy(p => p.Group).ThenBy(p => p.Name).ThenBy(p => p.DateReceipt);
            }
            else if (sort == "DateReceipt")
            {
                products = products.OrderByDescending(p => p.DateReceipt);
            }
            else if (sort == "Name")
            {
                products = products.OrderBy(p => p.Name);
            }

            /*return products != null ?
                        View(await products.ToListAsync()) :
                        Problem("Entity set 'ReceiptsContext.Products'  is null.");*/

            return products != null ?
                View(await PaginatedList<Products>.CreateAsync(products, pageNumber ?? 1, pageSizeDefault)) :
                Problem("Entity set 'ReceiptsContext.Products'  is null.");
        }

        // GET: GroupProducts
        public async Task<IActionResult> GroupProducts(string searchString, string filterGroup, string sort, string pageSize, int? pageNumber)
        {
            ViewData["searchString"] = searchString;
            ViewData["filterGroup"] = filterGroup;
            ViewData["sort"] = sort;

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
                if (sort == "Group")
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

                int pageSizeInt = pageSizeDefault;

                if (!pageSize.IsNullOrEmpty())
                {
                    pageSizeInt = int.Parse(pageSize);
                }

                //return View(await groupsProducts.ToListAsync());
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
            var groupSelectList = new List<SelectListItem>
            {
                new SelectListItem { Text = "", Value = "" }
            };
            foreach (var group in groupList)
            {
                if (filterGroup == group)
                {
                    groupSelectList.Add(new SelectListItem { Text = group, Value = group, Selected = true });
                }
                else
                {
                    groupSelectList.Add(new SelectListItem { Text = group, Value = group });
                }
            };
            return groupSelectList;
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
                new SelectListItem { Text = "Group", Value = "Group"},
                new SelectListItem { Text = "DateReceipt", Value = "DateReceipt"},
                new SelectListItem { Text = "Name", Value = "Name"}
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
                new SelectListItem { Text = "Group", Value = "Group"},
                new SelectListItem { Text = "PriceRatio", Value = "PriceRatio" },
                new SelectListItem { Text = "PricesCount", Value = "PricesCount" }
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
                new SelectListItem { Text = "20", Value = "20"},
                new SelectListItem { Text = "100", Value = "100" },
                new SelectListItem { Text = "All", Value = "100000" }
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

        public JsonResult LiveTagSearchJson(string search)
        {
            var res = (
                from t in _context.Products
                where t.Name.Contains(search)
                select new { t.Name }
                ).Distinct().Take(10).ToList();

            return Json(res);
        }
    }
}