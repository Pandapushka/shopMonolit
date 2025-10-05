using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shop.Common;
using shop.Model;
using shop.Model.Entitys.Order;
using shop.ModelDTO.OrderDTO;
using shop.Services.OrderService;

namespace shop.Controllers
{
    public class OrderController : StoreController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = $"{SharedData.Roles.Admin},{SharedData.Roles.Consumer}")]
        public async Task<ActionResult<ResponseServer<string>>> CreateFromCart(
             [FromBody] OrderCreateFromCartDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseServer<string>.Error("Неверные данные заказа"));
            try
            {
                var order = await _orderService.CreateOrderFromCartAsync(orderDto);
                return Ok(ResponseServer<string>.Success($"Заказ №{order.Id} успешно создан", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error($"Ошибка при создании заказа: {ex.Message}", 500));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(ResponseServer<Order>.Success(await _orderService.GetById(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<Order>.Error(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUserId(string id)
        {
            try
            {
                return Ok(ResponseServer<List<Order>>.Success(await _orderService.GetByUserId(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<Order>.Error(ex.Message));
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseServer<Order>.Error("Неверное состояние модели заказа"));
            }

            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, orderUpdateDTO);
                return Ok(ResponseServer<Order>.Success(updatedOrder, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<Order>.Error($"Произошла ошибка при обновлении заказа: {ex.Message}", 500));
            }
        }
    }
}
