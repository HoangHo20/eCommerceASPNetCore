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
    }
}
