using eCommerceASPNetCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eCommerceASPNetCore.Domain
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public ProductEnum Status { get; set; }

        [JsonIgnore]
        public virtual List<ProductImage> Images { get; set; } = new List<ProductImage>();

        [JsonIgnore]
        public virtual SubCategory SubCategory { get; set; }
    }
}
