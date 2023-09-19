using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace ReceiptsWeb.Models
{
	public class GroupProducts
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Name", ResourceType = typeof(Resources.SharedResource))]
		public string Name { get; set; }

		[Display(Name = "Group", ResourceType = typeof(Resources.SharedResource))]
		public string Group { get; set; }

		[Precision(18, 2)]
		[Display(Name = "Min", ResourceType = typeof(Resources.SharedResource))]
		public decimal Min { get; set; }

		[Precision(18, 2)]
		[Display(Name = "Max", ResourceType = typeof(Resources.SharedResource))]
		public decimal Max { get; set; }

		[Display(Name = "MinDate", ResourceType = typeof(Resources.SharedResource))]
		public DateTime MinDate { get; set; }

		[Display(Name = "MaxDate", ResourceType = typeof(Resources.SharedResource))]
		public DateTime MaxDate { get; set; }

		[Precision(18, 2)]
		[Display(Name = "PriceRatio", ResourceType = typeof(Resources.SharedResource))]
		public decimal PriceRatio { get; set; }

		[Display(Name = "PricesCount", ResourceType = typeof(Resources.SharedResource))]
		public int PricesCount { get; set; }
	}
}