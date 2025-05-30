using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Geno.Models;
using Geno.Services;

namespace Geno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string? name, int? id) => Ok(await _employeeService.FilterAsync(name));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            var created = await _employeeService.CreateAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = created.I3D }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Employee employee)
        {
            if (id != employee.I3D)
                return BadRequest("ID mismatch");

            var updatedEmployee = await _employeeService.UpdateAsync(employee);

            if (updatedEmployee == null)
                return NotFound();

            return Ok(updatedEmployee);
        }
    }
}