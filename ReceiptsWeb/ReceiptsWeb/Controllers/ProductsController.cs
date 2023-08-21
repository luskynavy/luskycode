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
        public async Task<IActionResult> Index()
        {
            return _context.Products != null ?
                        View(await _context.Products/*.Take(20)*/.OrderBy(p => p.Name).ThenBy(p => p.DateReceipt).ToListAsync()) :
                        Problem("Entity set 'ReceiptsContext.Products'  is null.");
        }

        // GET: GroupProducts
        public async Task<IActionResult> GroupProducts()
        {
            if (_context.Products != null)
            {
                //Method syntax
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
                                            Max = gp.Max(p => p.Price)
                                        });
                //Query syntax
                /*var groupsProducts = from p in _context.Products
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
                                         Max = gp.Max(x => x.Price)
                                     };*/
                return View(await groupsProducts.OrderBy(p => p.Group).ThenBy(p => p.Name).ToListAsync());
            }
            else
            {
                return Problem("Entity set 'ReceiptsContext.Products'  is null.");
            }
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