using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public List<SubCategory> SubCategories { get; set; }
    }
}
