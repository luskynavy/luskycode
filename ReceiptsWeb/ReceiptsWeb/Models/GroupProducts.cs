using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ReceiptsWeb.Models
{
    public class GroupProducts
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }

        [Precision(18, 2)]
        public decimal Min { get; set; }

        [Precision(18, 2)]
        public decimal Max { get; set; }
    }
}