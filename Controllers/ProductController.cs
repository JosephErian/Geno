using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Geno.Models;
using Geno.Services;

namespace Geno.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string? name) => Ok(await _productService.FilterAsync(name));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var created = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.I3D }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product product)
        {
            var existing = await _productService.UpdateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.I3D }, created);
        }
    }
}