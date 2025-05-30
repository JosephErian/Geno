using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Geno.Models;
using Geno.Services;

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

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string? name) => Ok(await _orderService.FilterAsync(name));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var created = await _orderService.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.I3D }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order order)
        {
            if (id != order.I3D)
                return BadRequest("ID mismatch");

            var updatedOrder = await _orderService.UpdateAsync(order);

            if (updatedOrder == null)
                return NotFound();

            return Ok(updatedOrder);
        }
    }
}