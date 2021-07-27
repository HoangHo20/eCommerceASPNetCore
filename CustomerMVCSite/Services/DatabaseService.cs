using CustomerMVCSite.Models;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Data;
using eCommerceASPNetCore.Domain;
using eCommerceASPNetCore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly eCommerceNetCoreContext _context;
        private readonly ICastService _castService;

        public DatabaseService(eCommerceNetCoreContext context, ICastService castService)
        {
            _context = context;
            _castService = castService;
        }

        // ====   Product table   ====
        public List<ProductModel> getAllProductAndItsProperties()
        {
            return _context.Products
                .Where(product => product.Status == ProductEnum.Available)
                .Include(product => product.Images)
                .Include(product => product.SubCategory)
                .Select(product => _castService.newProductModel(product, true, product.SubCategory.ID, product.Images, -1))
                .ToList();
        }

        public List<ProductModel> getAllProductsOnly(bool isGetDescription)
        {
            return _context.Products
                .Where(product => product.Status == ProductEnum.Available)
                .Select(product => _castService.newProductModel(product, isGetDescription, -1, null, -1))
                .ToList();
        }

        public List<Product> getAllProductAndProductImageOnly()
        {
            throw new NotImplementedException();
        }

        public List<Product> getAllProductCustomProperties(bool description, bool images, bool subcategory)
        {
            throw new NotImplementedException();
        }

        public Product getProductByID(int id)
        {
            return _context.Products
                .Where(product => product.Status == ProductEnum.Available && product.ID == id)
                .Include(product => product.Images)
                .Include(product => product.SubCategory)
                .FirstOrDefault();
        }

        public ProductModel getProductModelByID(int id)
        {
            return _context.Products
                .Where(product => product.Status == ProductEnum.Available && product.ID == id)
                .Include(product => product.Images)
                .Include(product => product.SubCategory)
                .Select(product => _castService.newProductModel(product, true, product.SubCategory.ID, product.Images, -1))
                .FirstOrDefault();
        }

        public IQueryable<Product> getProductsBySubcategoryID(int subcategoryId)
        {
            return _context.Products
                    .Where(product => product.Status == ProductEnum.Available)
                    .Include(product => product.SubCategory)
                    .Include(product => product.Images)
                    .Where(product => product.SubCategory.ID == subcategoryId)
                    .AsNoTracking();
        }

        public int createProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public int createProduct(Product product, SubCategory subCategory, List<string> imageUrls)
        {
            if (imageUrls != null)
            {
                foreach (string imgUrl in imageUrls)
                {
                    ProductImage productImage = new ProductImage()
                    {
                        Url = imgUrl
                    };

                    product.Images.Add(productImage);
                }
            }

            if (subCategory != null)
            {
                using (var context = new eCommerceNetCoreContext())
                {
                    product.SubCategory = subCategory;

                    context.Products.Add(product);

                    context.Entry(subCategory).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }

            return product.ID;
        }

        // ====   Category table   ====
        public List<CategoryModel> getAllCategoryOnly()
        {
            return _context.Categories
                .Where(category => category.status == CategoryEnum.Available)
                .Select(category => _castService.newCategoryModel(category))
                .ToList();
        }

        public List<Category> getAllCategoriesWithSubCategories()
        {
            return _context.Categories
                    .Where(category => category.status == CategoryEnum.Available)
                    .Include(category => category.SubCategories)
                    .ToList();
        }

        public List<Category> getAllCategory(bool isGetSubcategories = false, bool isGetProducts = false)
        {
            return _context.Categories
                .Where(category => category.status == CategoryEnum.Available)
                .Include(category => category.SubCategories)
                    .ThenInclude(subcategory => subcategory.Products)
                        .ThenInclude(product => product.Images)
                .ToList();
        }

        public CategoryModel getCategoryByName(string name)
        {
            if (name == null)
            {
                return null;
            }

            return _context.Categories
                .Where(category => category.status == CategoryEnum.Available && category.Name == name)
                .Select(category => _castService.newCategoryModel(category))
                .FirstOrDefault();
        }

        public CategoryModel getCategoryByID(int id)
        {
            return _context.Categories
                .Where(cate => cate.status == CategoryEnum.Available && cate.ID == id)
                .Select(cate => _castService.newCategoryModel(cate))
                .FirstOrDefault();
        }

        public async Task<CategoryModel> createCategory(CategoryModel categoryModel)
        {
            if (categoryModel == null || categoryModel.Name == null)
            {
                return null;
            }

            Category category = _castService.newCategory(categoryModel);

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return _castService.newCategoryModel(category);
        }

        public async Task<CategoryModel> updateCategory(CategoryModel categoryModel)
        {
            if (categoryModel.Name == null)
            {
                return null;
            }

            Category findCategory = _context.Categories
                .Where(cate => cate.ID == categoryModel.ID)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            findCategory.Name = categoryModel.Name;
            findCategory.Description = categoryModel.Description;

            await _context.SaveChangesAsync();

            return _castService.newCategoryModel(findCategory);
        }

        // ====   SubCategory table   ====
        public List<SubcategoryModel> getSubCategoriesByCategoryID(int id)
        {
            return _context.SubCategories
                .Where(subCategory => subCategory.status == CategoryEnum.Available && subCategory.Category.ID == id)
                .Select(SubCategory => _castService.newSubcategoryModel(SubCategory, id))
                .ToList();
        }

        public SubCategory getSubCategoryByID(int id, bool isGetProducts = false)
        {
            SubCategory subCategory = _context.SubCategories
                .TagWith("Get subcategory by ID")
                .Where(sub => sub.status == CategoryEnum.Available && sub.ID == id)
                .Select(subProp => new SubCategory { ID = subProp.ID, Name = subProp.Name })
                .FirstOrDefault();

            return subCategory;
        }

        public SubCategory getSubCategoryByName(string name, bool isGetProducts = false)
        {
            SubCategory subCategory;

            if (isGetProducts)
            {
                subCategory = _context.SubCategories
                    .TagWith("Get subcategory by Name")
                    .Where(sub => sub.status == CategoryEnum.Available && sub.Name.Equals(name))
                    .FirstOrDefault();
            }
            else
            {
                subCategory = _context.SubCategories
                    .TagWith("Get subcategory by Name")
                    .Where(sub => sub.status == CategoryEnum.Available && sub.Name.Equals(name))
                    .Select(subProp => new SubCategory
                    {
                        ID = subProp.ID,
                        Name = subProp.Name
                    })
                    .FirstOrDefault();
            }

            return subCategory;
        }

        public async Task<SubcategoryModel> createSubCategory(SubcategoryModel subcategoryModel)
        {
            if (subcategoryModel.Name == null)
            {
                return null;
            }

            Category findCategory = _context.Categories
                .Where(category => category.ID == subcategoryModel.CategoryId)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            SubCategory subCategory = _castService.newSubCategory(subcategoryModel);

            subCategory.Category = findCategory;

            _context.SubCategories.Add(subCategory);

            await _context.SaveChangesAsync();

            return _castService.newSubcategoryModel(subCategory, findCategory.ID);
        }

        public async Task<SubcategoryModel> updateSubCategory(SubcategoryModel subcategoryModel)
        {
            if (subcategoryModel.Name == null)
            {
                return null;
            }

            Category findCategory = _context.Categories
                .Where(category => category.ID == subcategoryModel.CategoryId)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            SubCategory findSubcategory = _context.SubCategories
                .Where(subCate => subCate.ID == subcategoryModel.ID)
                .FirstOrDefault();

            if (findSubcategory == null)
            {
                return null;
            }

            findSubcategory.Category = findCategory;
            findSubcategory.Name = subcategoryModel.Name;
            findSubcategory.Description = subcategoryModel.Description;

            await _context.SaveChangesAsync();

            return _castService.newSubcategoryModel(findSubcategory, findCategory.ID);
        }
    }
}
