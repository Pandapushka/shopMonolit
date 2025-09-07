using Microsoft.AspNetCore.Mvc;
using shop.Model;
using shop.Model.Entitys;
using shop.Model.Entitys.Order;
using shop.ModelDTO.OrderDTO;
using shop.Services.OrderService;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<ActionResult<ResponseServer<string>>> Create([FromBody] OrderCreateDTO orderCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseServer<string>.Error("Неверное состояник модели заказа"));
                 
            }
            try
            {
                var order = await _orderService.CreateOrderAsync(orderCreateDTO);
                return Ok(ResponseServer<string>.Success($"Заказ номер {order.Id} успешно создан", 200)); 
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ResponseServer<string>.Error(ex.Message, 500));
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
