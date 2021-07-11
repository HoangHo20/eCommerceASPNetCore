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
        
        [HttpGet]
        public IActionResult Product(int id)
        {
            var product = _databaseService.getProductByID(id);

            if (product != null)
            {
                return View(product);
            } else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
