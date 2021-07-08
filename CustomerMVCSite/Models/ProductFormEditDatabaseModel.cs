using eCommerceASPNetCore.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Models
{
    public class ProductFormEditDatabaseModel
    {
        public string input_product_name { get; set; }

        public string input_product_description { get; set; }

        public int input_product_price { get; set; }

        public int input_product_stock { get; set; }

        public int input_product_subcategory { get; set; }

        public List<IFormFile> input_product_images { get; set; }
    }
}
