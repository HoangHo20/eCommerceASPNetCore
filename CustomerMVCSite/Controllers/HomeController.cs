using CustomerMVCSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Domain;
using Microsoft.Extensions.Options;
using CustomerMVCSite.Options;
using eCommerceASPNetCore.Data;
using Microsoft.AspNetCore.Authorization;

namespace CustomerMVCSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseService _databaseService;
        
        public HomeController(ILogger<HomeController> logger,
            IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        public IActionResult Index()
        {
            List<Category> categories = _databaseService.getAllCategory(true, true);

            HomeViewModel homeModel = new HomeViewModel();
            homeModel.categories = categories;

            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> subcategory(int id, int? page)
        {
            var products = _databaseService.getProductsBySubcategoryID(id);

            if (products != null)
            {
                int currentPage = (int)(page > 0 ? page : 1);

                var pagingData = await PaginatedList<Product>.CreatePaging(products, currentPage, 3);

                return View(pagingData);
            }
            else
            {
                return View("Error");
            }
        }
        
        [HttpGet]
        public IActionResult Product(int id)
        {
            var product = _databaseService.getProductByID(id);

            if (product != null)
            {
                return View(product);
            } else
            {
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
