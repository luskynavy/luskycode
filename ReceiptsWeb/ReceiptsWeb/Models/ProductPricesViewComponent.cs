using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReceiptsWeb.Models
{
    public class ProductPricesViewComponent : ViewComponent
    {
        private readonly ReceiptsContext _context;

        public ProductPricesViewComponent(ReceiptsContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return View();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product != null)
            {
                var products = _context.Products.Where(m => m.Name == product.Name).OrderBy(p => p.DateReceipt);

                return await Task.FromResult((IViewComponentResult)View("ProductPrices", products));
            }

            return View();
        }
    }


}
