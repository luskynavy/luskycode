using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ReceiptsWebBlazor.Models;
using System.Text.RegularExpressions;

namespace ReceiptsWebBlazor.Components.Pages
{
    public partial class Products
    {
        // The products list
        private List<Products2>? productsList;

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

            var results = await products.Skip((Page - 1) * PageSize).Take(PageSize).ToArrayAsync();

            productsList = new List<Products2>();

            //Convert Product in Products and extract price per kilo
            foreach (var p in results)
            {
                var p2 = new Products2
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
                productsList.Add(p2);
            }

            Loading = false;
        }

        /// <summary>
        /// Extract the price per kilo from product name if possible
        /// </summary>
        /// <param name="name">product name with price</param>
        /// <param name="price">price of product</param>
        /// <returns></returns>
        public static decimal ExtractPricePerKilo(string name, decimal price)
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