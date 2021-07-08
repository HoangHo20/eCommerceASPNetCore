using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceASPNetCore.Domain;

namespace CustomerMVCSite.Services.Interface
{
    public interface IProductService
    {
        public List<Product> getAllProductAndProductImageOnly();

        public List<Product> getAllProductAndItsProperties();

        public List<Product> getAllProductCustomProperties(bool description,
            bool images,
            bool subcategory);

        public Product getProductByID(int ID);

        public int createProduct(Product product);

        public int createProduct(Product product, SubCategory subCategory, List<string> imageUrls);

    }
}
