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
        public IQueryable<Product> getProductsBySubcategoryID(int subcategoryId);
        public int createProduct(Product product);
        public int createProduct(Product product, SubCategory subCategory, List<string> imageUrls);
        
        // Category table
        public List<Category> getAllCategoryOnly();
        public List<Category> getAllCategoriesWithSubCategories();
        public List<Category> getAllCategory(bool isGetSubcategories, bool isGetProducts);
        public Category getCategoryByName(string name);
        public Task<Category> createCategory(Category category);
        public Task<Category> updateCategory(Category category);

        // SubCategory table
        public List<SubCategory> getSubCategoriesByCategoryID(int id);
        public SubCategory getSubCategoryByID(int id, bool isGetProducts = false);
        public SubCategory getSubCategoryByName(string name, bool isGetProducts = false);
        public Task<SubCategory> createSubCategory(int CategoryId, SubCategory subCategory);
        public Task<SubCategory> updateSubCategory(int CategoryId, SubCategory subCategory);
    }
}
