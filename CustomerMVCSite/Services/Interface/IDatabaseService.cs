using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services.Interface
{
    public interface IDatabaseService
    {
        // Product table
        public List<Product> getAllProductAndProductImageOnly();
        public List<Product> getAllProductAndItsProperties();
        public List<Product> getAllProductCustomProperties(bool description, bool images, bool subcategory);
        public Product getProductByID(int ID);
        public List<Product> getProductsBySubcategoryID(int subcategoryId);
        public int createProduct(Product product);
        public int createProduct(Product product, SubCategory subCategory, List<string> imageUrls);

        // Category table
        public List<Category> getAllCategoriesWithSubCategories();
        public List<Category> getAllCategory(bool isGetSubcategories, bool isGetProducts);

        // SubCategory table
        public SubCategory getSubCategoryByID(int id, bool isGetProducts = false);
        public SubCategory getSubCategoryByName(string name, bool isGetProducts = false);
    }
}
