using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eCommerceASPNetCore.Domain
{
    public class SubCategory
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual List<Product> Products { get; set; } = new List<Product>();

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [NotMapped]
        public int Category_ID { get; set; }

        public int status { get; set; }
    }
}
