using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Geno.Models;
using Geno.Services;
using Geno.Models.DTOs;

namespace Geno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string? name, int? id) => Ok(await _customerService.FilterAsync(name, id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto customer)
        {
            var created = await _customerService.CreateAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = created.I3D }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            if (id != customer.I3D)
                return BadRequest("ID mismatch");

            var updatedCustomer = await _customerService.UpdateAsync(customer);

            if (updatedCustomer == null)
                return NotFound();

            return Ok(updatedCustomer);
        }
    }
}