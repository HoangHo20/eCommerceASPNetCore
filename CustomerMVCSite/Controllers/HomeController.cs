﻿using CustomerMVCSite.Models;
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

namespace CustomerMVCSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICloudinaryService _cloudinaryService;

        public HomeController(ILogger<HomeController> logger, 
            IProductService productService,
            ICategoryService categoryService,
            ICloudinaryService cloudinaryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _cloudinaryService = cloudinaryService;
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

        [HttpGet]
        public IActionResult EditDatabase()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditDatabase([FromForm] ProductFormEditDatabaseModel product)
        {
            Console.WriteLine("helloworld");

            var result = await _cloudinaryService.UploadImage(product.input_product_images);

            return Ok(result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
