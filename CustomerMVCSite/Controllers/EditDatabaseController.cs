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
        private readonly IUploadService _uploadService;

        public EditDatabaseController(ILogger<EditDatabaseController> logger,
            IProductService productService,
            ICategoryService categoryService,
            IUploadService uploadService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _uploadService = uploadService;
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
                var subCategory = _categoryService.getSubCategoryByName(product.SubCategoryName, true);

                if (subCategory != null) {
                    List<string> imageUrls = null;

                    if (product.imageFiles != null)
                    {
                        imageUrls = await _uploadService.UploadImage(product.imageFiles);
                    }

                    int product_id = _productService.createProduct(product, subCategory, imageUrls);
                }

                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return BadRequest();
        }
    }
}
