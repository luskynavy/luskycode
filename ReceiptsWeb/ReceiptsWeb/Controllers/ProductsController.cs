using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceiptsWeb.Models;

namespace ReceiptsWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ReceiptsContext _context;

        public ProductsController(ReceiptsContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string currentFilter, string searchString, string filterGroup, string sort)
        {
            ViewData["CurrentFilter"] = searchString;

            //Select lists
            ViewBag.GroupList = GroupSelectList(filterGroup);
            ViewBag.ProductsSortList = ProductsSortList(sort);

            searchString ??= currentFilter;

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

            return products != null ?
                        View(await products.ToListAsync()) :
                        Problem("Entity set 'ReceiptsContext.Products'  is null.");
        }

        // GET: GroupProducts
        public async Task<IActionResult> GroupProducts(string currentFilter, string searchString, string filterGroup, string sort)
        {
            ViewData["CurrentFilter"] = searchString;

            //Select lists
            ViewBag.GroupList = GroupSelectList(filterGroup);
            ViewBag.GroupProductsSortList = GroupProductsSortList(sort);

            searchString ??= currentFilter;

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

                return View(await groupsProducts.ToListAsync());
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
        /// <param name="sort">selected value</param>
        /// <returns></returns>
        private List<SelectListItem> ProductsSortList(string sort)
        {
            var productsSortList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Group", Value = "Group"},
                new SelectListItem { Text = "DateReceipt", Value = "DateReceipt"},
                new SelectListItem { Text = "Name", Value = "Name"}
            };

            var selected = productsSortList.Find(p => p.Value == sort);
            if (selected != null)
            {
                selected.Selected = true;
            }

            return productsSortList;
        }

        /// <summary>
        /// Create group products select list for sort
        /// </summary>
        /// <param name="sort">selected value</param>
        /// <returns></returns>
        private List<SelectListItem> GroupProductsSortList(string sort)
        {
            var groupProductsSortList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Group", Value = "Group"},
                new SelectListItem { Text = "PriceRatio", Value = "PriceRatio" },
                new SelectListItem { Text = "PricesCount", Value = "PricesCount" }
            };

            var selected = groupProductsSortList.Find(p => p.Value == sort);
            if (selected != null)
            {
                selected.Selected = true;
            }

            return groupProductsSortList;
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
    }
}