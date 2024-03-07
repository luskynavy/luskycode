#nullable disable

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace ReceiptsWebBlazor.Models;

public class GroupProduct
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Group { get; set; }

    [Precision(18, 2)]
    public decimal Min { get; set; }

    [Precision(18, 2)]
    public decimal Max { get; set; }

    public DateTime MinDate { get; set; }

    public DateTime MaxDate { get; set; }

    [Precision(18, 2)]
    public decimal PriceRatio { get; set; }

    public int PricesCount { get; set; }
}