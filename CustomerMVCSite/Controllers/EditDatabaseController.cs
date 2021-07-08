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
    public class EditDatabaseController : Controller
    {
        private readonly ILogger<EditDatabaseController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICloudinaryService _cloudinaryService;

        public EditDatabaseController(ILogger<EditDatabaseController> logger,
            IProductService productService,
            ICategoryService categoryService,
            ICloudinaryService cloudinaryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public IActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Product([FromForm] ProductFormEditDatabaseModel product)
        {
            try
            {
                List<string> imageUrls = null;

                if (product.imageFiles != null)
                {
                    imageUrls = await _cloudinaryService.UploadImage(product.imageFiles);
                }

                var subCategory = await _categoryService.getSubCategoryByName(product.SubCategoryName, true);

                int product_id = _productService.createProduct(product, subCategory, imageUrls);

                return Ok($"ProductID:{product_id}");
            } catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return BadRequest();
        }
    }
}
