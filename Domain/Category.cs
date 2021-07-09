using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceASPNetCore.Domain
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

        public int status { get; set; }
    }
}
