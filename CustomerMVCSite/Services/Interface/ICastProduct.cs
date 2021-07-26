using CustomerMVCSite.Models;
using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services.Interface
{
    public interface ICastProduct
    {
        public Product newProduct(ProductModel productModel);
        public ProductModel newProductModel(Product product,
            bool isKeepDescription = true,
            int _subcategoryId = -1,
            List<ProductImage> productImages = null,
            int _categoryId = -1);
    }
}
