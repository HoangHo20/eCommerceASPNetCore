using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using eCommerceASPNetCore.Domain.Enums;

namespace eCommerceASPNetCore.Domain
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

        public CategoryEnum status { get; set; }
    }
}
