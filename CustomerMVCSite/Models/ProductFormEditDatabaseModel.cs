using eCommerceASPNetCore.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Models
{
    public class ProductFormEditDatabaseModel : Product
    {
        public string SubCategoryName { get; set; }

        public List<IFormFile> imageFiles { get; set; }
    }
}
