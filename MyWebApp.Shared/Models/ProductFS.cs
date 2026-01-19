using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Shared.Models
{
    public class ProductFS
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }=string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "Decimal(18,2)")]
        public decimal OldPrice { get; set; }

        public int Discount { get; set; } 

        public int Stars { get; set; } 

        public int Reviews { get; set; } 

        public string ImageUrl { get; set; } = string.Empty;
    }
}
