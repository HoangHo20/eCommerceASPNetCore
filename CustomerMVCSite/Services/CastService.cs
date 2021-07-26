using CustomerMVCSite.Models;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services
{
    public class CastService : ICastService
    {
        // Product cast
        public Product newProduct(ProductModel productModel)
        {
            if (productModel == null)
            {
                return null;
            }

            return new Product
            {
                ID = productModel.ID,
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                Stock = productModel.Stock
            };
        }

        public ProductModel newProductModel(Product product,
            bool isKeepDescription = true,
            int _subcategoryId = -1,
            List<ProductImage> productImages = null,
            int _categoryId = -1)
        {
            if (product == null)
            {
                return null;
            }

            return new ProductModel
            {
                ID = product.ID,
                Name = product.Name,
                Description = isKeepDescription ? product.Description : "",
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = _categoryId,
                SubcategoryId = _subcategoryId,
                ImageUrls = productImages == null ? null : productImages.Select(pi => pi.Url).ToList()
            };
        }

        // Category cast
        public Category newCategory(CategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                return null;
            }

            return new Category
            {
                ID = categoryModel.ID,
                Name = categoryModel.Name,
                Description = categoryModel.Description
            };
        }

        public CategoryModel newCategoryModel(Category category)
        {
            if (category == null)
            {
                return null;
            }

            return new CategoryModel
            {
                ID = category.ID,
                Name = category.Name,
                Description = category.Description
            };
        }

        // Subcategory cast
        public SubCategory newSubCategory(SubcategoryModel subcategoryModel)
        {
            if (subcategoryModel == null)
            {
                return null;
            }

            return new SubCategory
            {
                ID = subcategoryModel.ID,
                Name = subcategoryModel.Name,
                Description = subcategoryModel.Description
            };
        }

        public SubcategoryModel newSubcategoryModel(SubCategory subcategory, int categoryID = -1)
        {
            if (subcategory == null)
            {
                return null;
            }

            return new SubcategoryModel
            {
                ID = subcategory.ID,
                Name = subcategory.Name,
                Description = subcategory.Description,
                CategoryId = categoryID
            };
        }
    }
}
