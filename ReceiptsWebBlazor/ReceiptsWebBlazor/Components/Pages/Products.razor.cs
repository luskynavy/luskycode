using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ReceiptsWebBlazor.Models;

namespace ReceiptsWebBlazor.Components.Pages
{
    public partial class Products
    {
        // The products list
        private Product[]? productsList;

        // True if data are loading
        private bool Loading = true;

        // Current page
        public int Page { get; set; }

        // Current sort
        public string? Sort { get; set; }

        // Current filter by group
        public string? FilterGroup { get; set; }

        // Current filter by text on product name
        public string? SearchString { get; set; }

        // Total number of pages
        private int TotalPages { get; set; }

        #region Page size

        private int PageSize = 10;

        private async Task ChangePageSize(ChangeEventArgs e)
        {
            if (e.Value is not null)
            {
                int.TryParse(e.Value.ToString(), out PageSize);
                await ReloadAsync();
            }
        }

        #endregion Page size

        // Previous page management
        private bool HasPrev { get; set; }

        // Next page management
        private bool HasNext { get; set; }

        // Group list for select
        private List<string>? GroupList { get; set; }

        // Sort list for select
        private List<string>? SortList { get; set; }

        // Helper method to set disabled on class for paging.
        // condition: When the element is active (and therefore should not be disabled).
        //            Returns the string literal "disabled" or an empty string.
        private string IsDisabled(bool condition) =>
        !Loading && condition ? "" : "disabled";

        #region Popup

        // True if popup is displayed
        private bool ShowPopup = false;

        // Product Id to display in popup
        private int ShowPricesProductId;

        // Open the Popup
        private void ShowPrices(int productId)
        {
            ShowPopup = true;
            ShowPricesProductId = productId;
        }

        // Close the Popup
        private void ClosePopup()
        {
            ShowPopup = false;
        }

        #endregion Popup

        protected override async Task OnParametersSetAsync()
        {
            await ReloadAsync();

            await base.OnParametersSetAsync();
        }

        // Get text to display for sort value
        private string GetSortName(string sort)
        {
            switch (sort)
            {
                case "DateReceipt":
                    return "Date receipt";

                case "PriceRatio":
                    return "Price ratio";

                case "Name":
                    return "Name";

                default:
                    return "Group";
            }
        }

        private async Task ReloadAsync()
        {
            using var context = DbFactory.CreateDbContext();

            // Init select
            GroupList = context.Products.Select(p => p.Group).Distinct().ToList();
            SortList = new List<string>
            { "Group", "DateReceipt", "Name"};

            Loading = true;

            IQueryable<Product> products = context.Products;

            //Filter
            if (!String.IsNullOrEmpty(FilterGroup))
            {
                products = products.Where(s => s.Group.Equals(FilterGroup));
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.Name.Contains(SearchString));
            }

            //Sort
            if (Sort == "DateReceipt")
            {
                products = products.OrderByDescending(p => p.DateReceipt);
            }
            else if (Sort == "Name")
            {
                products = products.OrderBy(p => p.Name);
            }
            else //Group or unknown
            {
                products = products.OrderBy(p => p.Group).ThenBy(p => p.Name).ThenBy(p => p.DateReceipt);
            }

            var count = await products.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            if (Page > TotalPages)
            {
                Page = TotalPages;
            }
            if (Page <= 0)
            {
                Page = 1;
            }

            HasPrev = Page > 1;
            HasNext = Page < TotalPages;

            productsList = await products.Skip((Page - 1) * PageSize).Take(PageSize).ToArrayAsync();

            Loading = false;
        }

        //Clear button
        private async Task Clear()
        {
            Page = 1;
            Sort = null;
            FilterGroup = null;
            SearchString = null;
            PageSize = 10;

            await ReloadAsync();
        }

        #region Page management

        private async Task GoFirstPage()
        {
            Page = 1;

            await ReloadAsync();
        }

        private async Task GoPreviousPage()
        {
            Page--;

            await ReloadAsync();
        }

        private async Task GoNextPage()
        {
            Page++;

            await ReloadAsync();
        }

        private async Task GoLastPage()
        {
            Page = TotalPages;

            await ReloadAsync();
        }

        #endregion Page management
    }
}