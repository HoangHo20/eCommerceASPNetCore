using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Models
{
    public class HomeViewModel
    {
        public Product product { get; set; }

        public List<Product> products { get; set; }

        public List<Category> categories { get; set; }
    }
}
