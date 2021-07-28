using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Models
{
    public class ProductModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int SubcategoryId { get; set; }

        public int CategoryId { get; set; }

        public List<string> ImageUrls { get; set; }

        public List<IFormFile> Files { get; set; } 
    }
}
