using CustomerMVCSite.Models;
using CustomerMVCSite.Services.Interface;
using eCommerceASPNetCore.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseService _databaseService;

        public ProductController(ILogger<HomeController> logger,
            IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet("")] 
        public IEnumerable<ProductModel> Get()
        {
            return _databaseService.getAllProductsOnly(false);
        }

        [HttpGet("{id}")]
        public ProductModel Get(int id)
        {
            return _databaseService.getProductModelByID(id);
        }
    }
}
