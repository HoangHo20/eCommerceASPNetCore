using CustomerMVCSite.Models;
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
        public List<ProductModel> getAllProductAndItsProperties();
        public List<ProductModel> getAllProductsOnly(bool isGetDescription);
        public List<Product> getAllProductCustomProperties(bool description, bool images, bool subcategory);
        public Product getProductByID(int ID);
        public ProductModel getProductModelByID(int ID);
        public IQueryable<Product> getProductsBySubcategoryID(int subcategoryId);
        public int createProduct(Product product);
        public int createProduct(Product product, SubCategory subCategory, List<string> imageUrls);
        public Task<ProductModel> createProduct(ProductModel productModel);

        // Category table
        public List<CategoryModel> getAllCategoryOnly();
        public List<Category> getAllCategoriesWithSubCategories();
        public List<Category> getAllCategory(bool isGetSubcategories, bool isGetProducts);
        public CategoryModel getCategoryByName(string name);
        public CategoryModel getCategoryByID(int id);
        public Task<CategoryModel> createCategory(CategoryModel category);
        public Task<CategoryModel> updateCategory(CategoryModel category);

        // SubCategory table
        public List<SubcategoryModel> getSubCategoriesByCategoryID(int id);
        public SubcategoryModel getSubCategoryByID(int id);
        public SubCategory getSubCategoryByName(string name, bool isGetProducts = false);
        public Task<SubcategoryModel> createSubCategory(SubcategoryModel subCategory);
        public Task<SubcategoryModel> updateSubCategory(SubcategoryModel subCategory);
    }
}
