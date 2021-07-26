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
    public class CategoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseService _databaseService;

        public CategoryController(ILogger<HomeController> logger,
            IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet("")]
        public IEnumerable<CategoryModel> Get()
        {
            return _databaseService.getAllCategoryOnly();
        }

        [HttpGet("{id}")]
        public IEnumerable<SubcategoryModel> GetSubcategoryByCategory(int id)
        {
            return _databaseService.getSubCategoriesByCategoryID(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CategoryModel categoryModel)
        {
            try
            {
                if (categoryModel == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category is null");
                }

                if (_databaseService.getCategoryByName(categoryModel.Name) != null)
                {
                    _logger.LogError("Category existed or Category's name is null");
                    return BadRequest("Category existed or Category's name is null!");
                }

                categoryModel = await _databaseService.createCategory(categoryModel);

                if (categoryModel == null)
                {
                    _logger.LogError("Category cannot be created.");
                    return BadRequest("Category cannot be created");
                }

                return Ok(categoryModel);
            } catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Category Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] CategoryModel categoryModel)
        {
            try
            {
                if (categoryModel == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category is null");
                }

                categoryModel.ID = id;
                categoryModel = await _databaseService.updateCategory(categoryModel);

                if (categoryModel == null)
                {
                    _logger.LogError("Category cannot be create or Name is empty");
                    return BadRequest("Category cannot be create or Name is empty");
                }

                return Ok(categoryModel);
            } catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Category Put action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/subcategory/{subId}")]
        public IEnumerable<Product> GetProductsBySubcategory(int id, int subId)
        {
            return _databaseService.getProductsBySubcategoryID(subId)
                .Select(product => new Product
                {
                    ID = product.ID,
                    Name = product.Name,
                    Stock = product.Stock,
                    Price = product.Price
                })
                .ToList();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostSubcategoryToCategory(int id, [FromForm] SubcategoryModel subCategoryModel)
        {
            try
            {
                if (subCategoryModel == null)
                {
                    _logger.LogError("SubCategory object sent from client is null.");
                    return BadRequest("SubCategory is null");
                }

                if (_databaseService.getSubCategoryByName(subCategoryModel.Name, false) != null)
                {
                    _logger.LogError("subCategory existed or subCategory's name is null");
                    return BadRequest("subCategory existed or subCategory's name is null!");
                }

                subCategoryModel.CategoryId = id;
                subCategoryModel = await _databaseService.createSubCategory(subCategoryModel);

                if (subCategoryModel == null)
                {
                    _logger.LogError("subCategory cannot create or Category is not exist");
                    return BadRequest("subCategory cannot create or Category is not exist");
                }

                return Ok(subCategoryModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside subCategory Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/subcategory/{subId}")]
        public async Task<IActionResult> PutSubcategoryToCategory(int id, int subId, [FromForm] SubcategoryModel subCategoryModel)
        {
            try
            {
                if (subCategoryModel == null)
                {
                    _logger.LogError("subCategory object sent from client is null.");
                    return BadRequest("subCategory is null");
                }

                subCategoryModel.ID = subId;
                subCategoryModel.CategoryId = id;
                subCategoryModel = await _databaseService.updateSubCategory(subCategoryModel);

                if (subCategoryModel == null)
                {
                    _logger.LogError("subCategory cannot be update or Category is not exist.");
                    return BadRequest("subCategory cannot be update or Category is not exist.");
                }

                return Ok(subCategoryModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Category Put action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
