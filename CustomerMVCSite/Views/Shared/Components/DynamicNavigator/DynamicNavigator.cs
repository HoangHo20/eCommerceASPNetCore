using CustomerMVCSite.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Views.Shared.Components
{
    public class DynamicNavigator : ViewComponent
    {
        private readonly IDatabaseService _databaseService;

        public DynamicNavigator(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _databaseService.getAllCategoriesWithSubCategories();
            return View(categories);
        }
    }
}
