using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using shop.Common;
using shop.Data;
using shop.Model;
using shop.Model.Entitys;
using shop.ModelDTO;
using shop.ModelDTO.UserDTO;
using shop.Services.ProductService;
using shop.Services.Storage;
using shop.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace shop.Controllers
{
    public class ProductController : StoreController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(ResponseServer<IEnumerable<Product>>.Success(await _productService.GetAll()));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<IEnumerable<Product>>.Error("Произошла ошибка при получении данных"));
            }

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(ResponseServer<Product>.Success(await _productService.GetById(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<Product>.Error(ex.Message));
            }
        }
        [HttpPost("Create")]
        [Authorize(Roles = $"{SharedData.Roles.Admin},{SharedData.Roles.Consumer}")]
        public async Task<ActionResult<ResponseServer<string>>> Create(
            [FromForm] ProductCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                    return BadRequest(ResponseServer<string>.Error("Модель продукта не может быть пустой", 400));
                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<string>.Error("Некорректные данные валидации", 400));
                await _productService.Create(createDTO);
                return Ok(ResponseServer<string>.Success("Продукт успешно создан", 201));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error("Произошла ошибка при создании продукта", 500));
            }
        }

        [Authorize(Roles = $"{SharedData.Roles.Admin}")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseServer<string>>> Update(int id, [FromForm] ProductCreateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null)
                    return BadRequest(ResponseServer<string>.Error("Модель продукта не может быть пустой", 400));

                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<string>.Error("Некорректные данные валидации", 400));

                await _productService.Update(id, updateDTO);
                return Ok(ResponseServer<string>.Success("Продукт успешно обновлен", 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error("Произошла ошибка при обновлении продукта", 500));
            }
        }
        [Authorize(Roles = $"{SharedData.Roles.Admin}")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseServer<string>>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ResponseServer<string>.Error("Некорректный ID", 400));

                await _productService.Delete(id);
                return Ok(ResponseServer<string>.Success("Продукт успешно удален", 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error("Произошла ошибка при удалении продукта", 500));
            }
        }
    }
}
