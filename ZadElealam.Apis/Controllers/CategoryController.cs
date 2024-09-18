using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZadElealam.Core.Dto;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("getallCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoery();
            return Ok(categories);
        }
        
        [HttpGet("getCategoryById")]
        public async Task<IActionResult> GetCategoryById(int CategoryId)
        {
            var category = await _categoryRepository.GetCategoeryById(CategoryId);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory([FromForm] CategoryDto category)
        {
            var response = await _categoryRepository.AddCategoery(category);
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromForm] CategoryDto category)
        {
            var response = await _categoryRepository.UpdateCategoery(categoryId, category);
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("deleteCategory")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var response = await _categoryRepository.DeleteCategoery(categoryId);
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
