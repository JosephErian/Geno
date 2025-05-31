using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Geno.Models;
using Geno.Services;
using Geno.Models.DTOs;

namespace Geno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            var order = await _orderService.GetByCustomerId(customerId);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string? name) => Ok(await _orderService.FilterAsync(name));

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateAsync([FromBody] CreateOrderDto order)
        {
            var result = await _orderService.CreateOrUpdateAsync(order);
            if (result == null) return BadRequest("Invalid customer, employee, or product data.");

            if (order.I3D == 0)
            {
                // New order was created
                return CreatedAtAction(nameof(GetById), new { id = result.I3D }, result);
            }

            return Ok(result);
        }
    }
}