using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReceiptsWeb.Models
{
	[ModelMetadataType(typeof(ProductsExtend))]
	public partial class Products
	{
	}

	public class ProductsExtend
	{
		[Display(Name = "Name", ResourceType = typeof(Resources.SharedResource))]
		public string Name { get; set; }

		[Display(Name = "Group", ResourceType = typeof(Resources.SharedResource))]
		public string Group { get; set; }

		[Display(Name = "Price", ResourceType = typeof(Resources.SharedResource))]
		public decimal Price { get; set; }

		[Display(Name = "DateReceipt", ResourceType = typeof(Resources.SharedResource))]
		public DateTime DateReceipt { get; set; }

		[Display(Name = "SourceName", ResourceType = typeof(Resources.SharedResource))]
		public string SourceName { get; set; }

		[Display(Name = "SourceLine", ResourceType = typeof(Resources.SharedResource))]
		public int SourceLine { get; set; }

		[Display(Name = "FullData", ResourceType = typeof(Resources.SharedResource))]
		public string FullData { get; set; }
	}
}