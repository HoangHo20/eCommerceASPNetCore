using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace CustomerMVCSite.Services.Interface
{
    public interface IProductService
    {
        public List<Product> getAllProductAndProductImageOnly();

        public List<Product> getAllProductAndItsProperties();

        public List<Product> getAllProductCustomProperties(bool description,
            bool images,
            bool subcategory);
    }
}
