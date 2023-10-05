using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ReceiptsWeb.Models
{
	public class ProductsPrices
	{
		public decimal Price { get; set; }

		public DateTime DateReceipt { get; set; }
	}
}