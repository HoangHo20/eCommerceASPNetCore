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
            throw new NotImplementedException();
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
            using (var context = new eCommerceNetCoreContext())
            {
                return context.Products
                    .Where(product => product.ID == id)
                    .Include(product => product.Images)
                    .Include(product => product.SubCategory)
                    .FirstOrDefault();
            }
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
        public List<Category> getAllCategoriesWithSubCategories()
        {
            using (var context = new eCommerceNetCoreContext())
            {
                return context.Categories
                    .Include(category => category.SubCategories)
                    .ToList();
            }
        }

        public List<Category> getAllCategory(bool isGetSubcategories = false, bool isGetProducts = false)
        {
            List<Category> categories;

            using (var context = new eCommerceNetCoreContext())
            {
                categories = context.Categories
                    .Include(category => category.SubCategories)
                        .ThenInclude(subcategory => subcategory.Products)
                            .ThenInclude(product => product.Images)
                    .ToList();
            }

            return categories;
        }


        // ====   SubCategory table   ====
        public SubCategory getSubCategoryByID(int id, bool isGetProducts = false)
        {
            using (var context = new eCommerceNetCoreContext())
            {
                SubCategory subCategory = context.SubCategories
                    .TagWith("Get subcategory by ID")
                    .Where(sub => sub.ID == id)
                    .Select(subProp => new SubCategory { ID = subProp.ID, Name = subProp.Name })
                    .FirstOrDefault();

                return subCategory;
            }
        }

        public SubCategory getSubCategoryByName(string name, bool isGetProducts = false)
        {
            using (var context = new eCommerceNetCoreContext())
            {
                SubCategory subCategory;

                if (isGetProducts)
                {
                    subCategory = context.SubCategories
                        .TagWith("Get subcategory by Name")
                        .Where(sub => sub.Name.Equals(name))
                        .FirstOrDefault();
                }
                else
                {
                    subCategory = context.SubCategories
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
        }
    }
}
