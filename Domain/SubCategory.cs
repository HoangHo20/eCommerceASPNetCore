using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceASPNetCore.Domain
{
    public class SubCategory
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public Category Category { get; set; }
    }
}
