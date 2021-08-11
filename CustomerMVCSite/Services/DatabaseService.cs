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
                product.SubCategory = subCategory;

                _context.Products.Add(product);

                _context.Entry(subCategory).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return product.ID;
        }

        public async Task<ProductModel> createProduct(ProductModel productModel)
        {
            Product newProduct = _castService.newProduct(productModel);

            if (productModel.ImageUrls == null)
            {
                return null;
            }

            SubCategory findSubcategory = _context.SubCategories
                .Where(subCate => subCate.status == CategoryEnum.Available && subCate.ID == productModel.SubcategoryId)
                .FirstOrDefault();

            if (findSubcategory == null)
            {
                return null;
            }

            foreach (string url in productModel.ImageUrls)
            {
                newProduct.Images.Add(new ProductImage
                {
                    Url = url
                });
            }

            newProduct.SubCategory = findSubcategory;

            _context.Products.Add(newProduct);
            _context.Entry(findSubcategory).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _castService.newProductModel(newProduct, false, findSubcategory.ID, null, -1);
        }

        public async Task<bool> deleteProduct(int id)
        {
            try
            {
                Product findProduct = _context.Products
                .Where(prod => prod.Status == ProductEnum.Available && prod.ID == id)
                .Include(prod => prod.Images)
                .FirstOrDefault();

                if (findProduct == null)
                {
                    return false;
                }

                findProduct.Status = ProductEnum.Deleted;

                await _context.SaveChangesAsync();

                return true;
            } catch (Exception ex)
            {
                return false;
            }
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
                    .Include(category => category.SubCategories.Where(subcate => subcate.status == CategoryEnum.Available))
                    .ToList();
        }

        public List<Category> getAllCategory(bool isGetSubcategories = false, bool isGetProducts = false)
        {
            return _context.Categories
                .Where(cate => cate.status == CategoryEnum.Available)
                .Include(category => category.SubCategories.Where(subcate => subcate.status == CategoryEnum.Available))
                    .ThenInclude(subcategory => subcategory.Products.Where(prod => prod.Status == ProductEnum.Available).Take(6))
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
                .Where(cate => cate.status == CategoryEnum.Available && cate.ID == categoryModel.ID)
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

        public async Task<bool> deleteCategory(int id)
        {
            try
            {
                Category findCategory = _context.Categories
                .Where(cate => cate.status == CategoryEnum.Available && cate.ID == id)
                .Include(cate => cate.SubCategories.Where(sub => sub.status == CategoryEnum.Available))
                    .ThenInclude(subcategory => subcategory.Products.Where(pro => pro.Status == ProductEnum.Available))
                .FirstOrDefault();

                if (findCategory == null)
                {
                    return false;
                }

                findCategory.status = CategoryEnum.Deleted;

                foreach (SubCategory subcate in findCategory.SubCategories)
                {
                    subcate.status = CategoryEnum.Deleted;

                    foreach (Product prod in subcate.Products)
                    {
                        prod.Status = ProductEnum.Deleted;
                    }
                }

                await _context.SaveChangesAsync();

                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        // ====   SubCategory table   ====
        public List<SubcategoryModel> getSubCategoriesByCategoryID(int id)
        {
            return _context.SubCategories
                .Where(subCategory => subCategory.status == CategoryEnum.Available && subCategory.Category.ID == id)
                .Select(SubCategory => _castService.newSubcategoryModel(SubCategory, id))
                .ToList();
        }

        public SubcategoryModel getSubCategoryByID(int id)
        {
            return _context.SubCategories
                .TagWith("Get subcategory by ID")
                .Where(sub => sub.status == CategoryEnum.Available && sub.ID == id)
                .Include(sub => sub.Category)
                .Select(sub => _castService.newSubcategoryModel(sub, sub.Category.ID))
                .FirstOrDefault();
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
                .Where(category => category.status == CategoryEnum.Available && category.ID == subcategoryModel.CategoryId)
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
                .Where(category => category.status == CategoryEnum.Available && category.ID == subcategoryModel.CategoryId)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            SubCategory findSubcategory = _context.SubCategories
                .Where(subCate => subCate.status == CategoryEnum.Available && subCate.ID == subcategoryModel.ID)
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

        public async Task<bool> deleteSubcategory(int id)
        {
            try
            {
                SubCategory findSubcategory = _context.SubCategories
                .Where(sub => sub.status == CategoryEnum.Available && sub.ID == id)
                .Include(sub => sub.Products)
                .FirstOrDefault();

                if (findSubcategory == null)
                {
                    return false;
                }

                findSubcategory.status = CategoryEnum.Deleted;

                foreach (Product p in findSubcategory.Products)
                {
                    p.Status = ProductEnum.Deleted;
                }

                await _context.SaveChangesAsync();

                return true;
            } catch(Exception Ignored)
            {
                return false;
            }
        }
    }
}
