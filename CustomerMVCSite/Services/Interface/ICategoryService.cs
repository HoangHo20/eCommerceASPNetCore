using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services.Interface
{
    public interface ICategoryService
    {
        public List<Category> getAllCategoriesNameOnly();

        public List<Category> getAllCategoriesWithSubCategories();

        public List<Category> getAllCategory(bool isGetSubcategories, bool isGetProducts);

        public SubCategory getSubCategoryByID(int id, bool isGetProducts = false);

        public SubCategory getSubCategoryByName(string name, bool isGetProducts = false);
    }
}
