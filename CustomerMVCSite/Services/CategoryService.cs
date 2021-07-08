using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Data;
using eCommerceASPNetCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerMVCSite.Services
{
    public class CategoryService : ICategoryService
    {
        public List<Category> getAllCategoriesNameOnly()
        {
            return createDumbData(false);
        }

        public List<Category> getAllCategoriesWithSubCategories()
        {
            return createDumbData(true);
        }

        public async Task<SubCategory> getSubCategoryByID(int id, bool isGetProducts = false)
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

        public async Task<SubCategory> getSubCategoryByName(string name, bool isGetProducts = false)
        {
            using (var context = new eCommerceNetCoreContext())
            {
                SubCategory subCategory;

                if (isGetProducts) {
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
                        .Select(subProp => new SubCategory { ID = subProp.ID, 
                            Name = subProp.Name })
                        .FirstOrDefault();
                }

                return subCategory;
            }
        }

        // Dumb data
        private List<Category> createDumbData(bool hasSubCategories = false)
        {
            string[] categoryName = { "CPU", "PSU", "Ram", "VGA" };
            string[] subCategoryName =
            {
                "Ryzen 5",
                "Core I3",
                "Core I5",
                "60plus",
                "80plus",
                "90plus",
                "4GB",
                "8GB",
                "16GB",
                "Gigabyte",
                "Ryzen RX",
                "Asus"
            };

            List<Category> categories = new List<Category>();

            // Each category has 3 subcategories
            int subCategoryGotCount = 0;
            for (int i=0; i < categoryName.Length; i++)
            {
                Category cate = new Category()
                {
                    ID = i,
                    Name = categoryName[i]
                };

                if (hasSubCategories)
                {
                    cate.SubCategories = new List<SubCategory>();

                    for (int m = 0; m < 3; m++)
                    {
                        SubCategory subCate = new SubCategory()
                        {
                            ID = subCategoryGotCount,
                            Name = subCategoryName[subCategoryGotCount]
                        };

                        cate.SubCategories.Add(subCate);
                        subCategoryGotCount++;
                    }
                }

                categories.Add(cate);
            }

            return categories;
        }
    }
}
