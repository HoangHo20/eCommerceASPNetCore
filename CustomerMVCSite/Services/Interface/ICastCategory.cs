using CustomerMVCSite.Models;
using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services.Interface
{
    public interface ICastCategory
    {
        public Category newCategory(CategoryModel categoryModel);
        public CategoryModel newCategoryModel(Category category);
    }
}
