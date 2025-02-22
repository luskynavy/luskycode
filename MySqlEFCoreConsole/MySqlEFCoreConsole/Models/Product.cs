using System;
using System.Collections.Generic;

namespace MySqlEFCoreConsole.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Group { get; set; }

    public decimal Price { get; set; }

    public DateTime DateReceipt { get; set; }

    public string SourceName { get; set; } = null!;

    public int SourceLine { get; set; }

    public string FullData { get; set; } = null!;
}
