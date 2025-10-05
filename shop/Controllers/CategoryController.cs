using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop.Common;
using shop.Model;
using shop.ModelDTO.CategoryDTO;
using shop.Services.CategoryService;

namespace shop.Controllers
{
    public class CategoryController : StoreController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ActionName("GetAllCategories")]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync(includeInactive);
                return Ok(ResponseServer<List<CategoryResponseDTO>>.Success(categories));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(ResponseServer<CategoryResponseDTO>.Success(category));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<string>.Error("Некорректные данные"));

                var category = await _categoryService.CreateCategoryAsync(categoryDto);
                return Ok(ResponseServer<CategoryResponseDTO>.Success(category, 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error("Ошибка при создании категории"));
            }
        }

        [HttpPut("{id:int}")]
        [ActionName("Update")]
        //[Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<string>.Error("Некорректные данные"));

                var category = await _categoryService.UpdateCategoryAsync(id, categoryDto);
                return Ok(ResponseServer<CategoryResponseDTO>.Success(category, 200));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(ResponseServer<string>.Success("Категория удалена"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<string>.Error(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error("Ошибка при удалении категории"));
            }
        }

        [HttpPatch("{id:int}/status")]
        //[Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool isActive)
        {
            try
            {
                await _categoryService.ToggleCategoryStatusAsync(id, isActive);
                var message = isActive ? "Категория активирована" : "Категория деактивирована";
                return Ok(ResponseServer<string>.Success(message));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error("Ошибка при изменении статуса"));
            }
        }
    }
}
