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
        private readonly IUploadService _uploadService;

        public ProductController(ILogger<HomeController> logger,
            IDatabaseService databaseService,
            IUploadService uploadService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _uploadService = uploadService;
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

        [HttpPost]
        public async Task<IActionResult> createProduct([FromForm] ProductModel productModel)
        {
            if (productModel == null)
            {
                return BadRequest("Request body empty");
            }

            if (productModel.Name == null || productModel.Name.Length < 1)
            {
                return BadRequest("Product's Name cannot be empty");
            }

            if (productModel.Files.Count < 1)
            {
                return BadRequest("Image Files list cannot be empty");
            } else
            {
                List<string> imgUrls = await _uploadService.UploadImage(productModel.Files);

                productModel.ImageUrls = imgUrls;

                productModel = await _databaseService.createProduct(productModel);

                if (productModel == null)
                {
                    return BadRequest("Cannot create product");
                }

                return Ok(productModel);
            }
        }
    }
}
