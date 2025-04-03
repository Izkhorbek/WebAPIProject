using ECommerceApp.DTOs.CategoryDTOs;
using ECommerceApp.DTOs;
using ECommerceApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Creates a new category.
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> CreateCategory([FromBody] CategoryCreateDTO categoryDto)
        {
            var response = await _categoryService.CreateCategoryAsync(categoryDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves a category by ID.
        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Updates an existing category.
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCategory([FromBody] CategoryUpdateDTO categoryDto)
        {
            var response = await _categoryService.UpdateCategoryAsync(categoryDto);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Deletes a category by ID.
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves all categories.
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ApiResponse<List<CategoryResponseDTO>>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

    }
}
