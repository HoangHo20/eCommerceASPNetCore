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
        private readonly ILogger<CategoryController> _logger;
        private readonly IDatabaseService _databaseService;
        private readonly ICastService _castService;

        public CategoryController(ILogger<CategoryController> logger,
            IDatabaseService databaseService,
            ICastService castService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _castService = castService;
        }

        [HttpGet("")]
        public IEnumerable<CategoryModel> Get()
        {
            return _databaseService.getAllCategoryOnly();
        }


        [HttpGet("name/{name}")]
        public IActionResult GetCategoryByName(string name)
        {
            if (name == null)
            {
                _logger.LogError("Category object sent from client is null.");
                return BadRequest("Category's Name is null");
            }

            return Ok(_databaseService.getCategoryByName(name));
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryByCategory(int id)
        {
            return Ok(_databaseService.getCategoryByID(id));
        }

        [HttpPost()]
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
        public async Task<IActionResult> Delete(int id)
        {
            if (await _databaseService.deleteCategory(id))
            {
                return Ok(new CategoryModel
                {
                    ID = id
                });
            }

            return BadRequest("Cannot delete Category");
        }

        [HttpGet("{id}/subcategory")]
        public IEnumerable<SubcategoryModel> GetSubcategoryByCategory(int id)
        {
            return _databaseService.getSubCategoriesByCategoryID(id);
        }

        [HttpPost("{id}/subcategory")]
        public async Task<IActionResult> PostSubcategoryToCategory(int id, [FromForm] SubcategoryModel subCategoryModel)
        {
            try
            {
                if (subCategoryModel == null)
                {
                    _logger.LogError("SubCategory object sent from client is null.");
                    return BadRequest("SubCategory is null");
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

        [HttpGet("subcategory/{subId}")]
        public IActionResult getSubcategoryById(int subId)
        {
            return Ok(_databaseService.getSubCategoryByID(subId));
        }

        [HttpDelete("subcategory/{subId}")]
        public async Task<IActionResult> deleteSubcategoryById(int subId)
        {
            if (await _databaseService.deleteSubcategory(subId))
            {
                return Ok(new SubcategoryModel
                {
                    ID = subId
                });
            }

            return BadRequest("Cannot delete Subcategory");
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

        [HttpGet("subcategory/{subId}/product")]
        public IEnumerable<ProductModel> GetProductsBySubcategory(int subId)
        {
            return _databaseService.getProductsBySubcategoryID(subId)
                .Select(product => _castService.newProductModel(product, false, -1, null, -1))
                .ToList();
        }
    }
}
