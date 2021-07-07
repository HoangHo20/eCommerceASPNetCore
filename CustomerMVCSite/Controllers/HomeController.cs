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

namespace CustomerMVCSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, 
            IProductService productService,
            ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            List<Category> categories = _categoryService.getAllCategoriesWithSubCategories();
            List<Product> products = _productService.getAllProductAndProductImageOnly();

            HomeViewModel homeModel = new HomeViewModel();
            homeModel.categories = categories;
            homeModel.products = products;

            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Product(int id)
        {
            List<Category> categories = _categoryService.getAllCategoriesWithSubCategories();
            List<Product> products = _productService.getAllProductAndProductImageOnly();
            Product product = _productService.getProductByID(id);

            HomeViewModel homeModel = new HomeViewModel();
            homeModel.categories = categories;
            homeModel.product = product;
            homeModel.products = products;

            return View(homeModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
