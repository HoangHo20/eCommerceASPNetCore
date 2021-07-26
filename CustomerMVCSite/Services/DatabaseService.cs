using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Data;
using eCommerceASPNetCore.Domain;
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

        public DatabaseService(eCommerceNetCoreContext context)
        {
            _context = context;
        }

        // ====   Product table   ====
        public List<Product> getAllProductAndItsProperties()
        {
            return _context.Products
                .Include(product => product.Images)
                .Include(product => product.SubCategory)
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
                .Where(product => product.ID == id)
                .Include(product => product.Images)
                .Include(product => product.SubCategory)
                .FirstOrDefault();
        }

        public IQueryable<Product> getProductsBySubcategoryID(int subcategoryId)
        {
            return _context.Products
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
        public List<Category> getAllCategoryOnly()
        {
            return _context.Categories.ToList();
        }

        public List<Category> getAllCategoriesWithSubCategories()
        {
            return _context.Categories
                    .Include(category => category.SubCategories)
                    .ToList();
        }

        public List<Category> getAllCategory(bool isGetSubcategories = false, bool isGetProducts = false)
        {
            return _context.Categories
                .Include(category => category.SubCategories)
                    .ThenInclude(subcategory => subcategory.Products)
                        .ThenInclude(product => product.Images)
                .ToList();
        }

        public Category getCategoryByName(string name)
        {
            if (name == null)
            {
                return null;
            }

            return _context.Categories
                .Where(category => category.Name == name)
                .FirstOrDefault();
        }

        public async Task<Category> createCategory(Category category)
        {
            if (category == null || category.Name == null)
            {
                return null;
            }

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> updateCategory(Category category)
        {
            if (category.Name == null)
            {
                return null;
            }

            Category findCategory = _context.Categories
                .Where(cate => cate.ID == category.ID)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            findCategory.Name = category.Name;
            findCategory.Description = category.Description;

            await _context.SaveChangesAsync();

            return findCategory;
        }

        // ====   SubCategory table   ====
        public List<SubCategory> getSubCategoriesByCategoryID(int id)
        {
            return _context.SubCategories
                .Where(subCategory => subCategory.Category.ID == id)
                .ToList();
        }

        public SubCategory getSubCategoryByID(int id, bool isGetProducts = false)
        {
            SubCategory subCategory = _context.SubCategories
                .TagWith("Get subcategory by ID")
                .Where(sub => sub.ID == id)
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
                    .Where(sub => sub.Name.Equals(name))
                    .FirstOrDefault();
            }
            else
            {
                subCategory = _context.SubCategories
                    .TagWith("Get subcategory by Name")
                    .Where(sub => sub.Name.Equals(name))
                    .Select(subProp => new SubCategory
                    {
                        ID = subProp.ID,
                        Name = subProp.Name
                    })
                    .FirstOrDefault();
            }

            return subCategory;
        }

        public async Task<SubCategory> createSubCategory(int CategoryId, SubCategory subCategory)
        {
            if (subCategory.Name == null)
            {
                return null;
            }

            Category findCategory = _context.Categories
                .Where(category => category.ID == CategoryId)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            subCategory.Category = findCategory;

            _context.SubCategories.Add(subCategory);

            await _context.SaveChangesAsync();

            return subCategory;
        }

        public async Task<SubCategory> updateSubCategory(int CategoryId, SubCategory subCategory)
        {
            if (subCategory.Name == null)
            {
                return null;
            }

            Category findCategory = _context.Categories
                .Where(category => category.ID == CategoryId)
                .FirstOrDefault();

            if (findCategory == null)
            {
                return null;
            }

            SubCategory findSubcategory = _context.SubCategories
                .Where(subCate => subCate.ID == subCategory.ID)
                .FirstOrDefault();

            if (findSubcategory == null)
            {
                return null;
            }

            findSubcategory.Category = findCategory;
            findSubcategory.Name = subCategory.Name;
            findSubcategory.Description = subCategory.Description;

            await _context.SaveChangesAsync();

            return new SubCategory
            {
                ID = findSubcategory.ID,
                Name = findSubcategory.Name,
                Description = findSubcategory.Description,
                Category_ID = findCategory.ID
            };
        }
    }
}
