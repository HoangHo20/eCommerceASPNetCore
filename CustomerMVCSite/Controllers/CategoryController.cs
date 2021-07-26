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
        public IEnumerable<Category> Get()
        {
            return _databaseService.getAllCategoryOnly();
        }

        [HttpGet("{id}")]
        public IEnumerable<SubCategory> GetSubcategoryByCategory(int id)
        {
            return _databaseService.getSubCategoriesByCategoryID(id);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category is null");
                }

                if (_databaseService.getCategoryByName(category.Name) != null)
                {
                    _logger.LogError("Category existed or Category's name is null");
                    return BadRequest("Category existed or Category's name is null!");
                }

                category = await _databaseService.createCategory(category);

                if (category == null)
                {
                    _logger.LogError("Category cannot be created.");
                    return BadRequest("Category cannot be created");
                }
            } catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Category Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Category category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category is null");
                }

                category.ID = id;
                category = await _databaseService.updateCategory(category);

                if (category == null)
                {
                    _logger.LogError("Category cannot be create or Name is empty");
                    return BadRequest("Category cannot be create or Name is empty");
                }

                return Ok(category);
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
        public async Task<IActionResult> PostSubcategoryToCategory(int id, [FromBody] SubCategory subCategory)
        {
            try
            {
                if (subCategory == null)
                {
                    _logger.LogError("SubCategory object sent from client is null.");
                    return BadRequest("SubCategory is null");
                }

                if (_databaseService.getSubCategoryByName(subCategory.Name, false) != null)
                {
                    _logger.LogError("subCategory existed or subCategory's name is null");
                    return BadRequest("subCategory existed or subCategory's name is null!");
                }

                subCategory = await _databaseService.createSubCategory(id, subCategory);

                if (subCategory == null)
                {
                    _logger.LogError("subCategory cannot create or Category is not exist");
                    return BadRequest("subCategory cannot create or Category is not exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside subCategory Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(subCategory);
        }

        [HttpPut("{id}/subcategory/{subId}")]
        public async Task<IActionResult> PutSubcategoryToCategory(int id, int subId, [FromBody] SubCategory subCategory)
        {
            try
            {
                if (subCategory == null)
                {
                    _logger.LogError("subCategory object sent from client is null.");
                    return BadRequest("subCategory is null");
                }

                subCategory.ID = subId;
                subCategory = await _databaseService.updateSubCategory(id, subCategory);

                if (subCategory == null)
                {
                    _logger.LogError("subCategory cannot be update or Category is not exist.");
                    return BadRequest("subCategory cannot be update or Category is not exist.");
                }

                return Ok(subCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Category Put action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
