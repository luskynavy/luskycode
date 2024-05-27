using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ReceiptsBlazorWinForms.Models;

public class GroupProduct
{
	[Key]
	public int Id { get; set; }

	public string? Name { get; set; }

	public string? Group { get; set; }

	[Precision(18, 2)]
	public decimal Min { get; set; }

	[Precision(18, 2)]
	public decimal Max { get; set; }

	[Precision(18, 2)]
	public decimal PreviousPrice { get; set; }

	[Precision(18, 2)]
	public decimal LastPrice { get; set; }

	[Precision(18, 2)]
	public decimal LastPricePerKilo { get; set; }

	public DateTime MinDate { get; set; }

	public DateTime MaxDate { get; set; }

	[Precision(18, 2)]
	public decimal PriceRatio { get; set; }

	public int PricesCount { get; set; }

	public IEnumerable<decimal> PricesList { get; set; }
}