using CustomerMVCSite.Models;
using eCommerceASPNetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services.Interface
{
    public interface ICastSubcategory
    {
        public SubCategory newSubCategory(SubcategoryModel categoryModel);
        public SubcategoryModel newSubcategoryModel(SubCategory category, int categoryID = -1);
    }
}
