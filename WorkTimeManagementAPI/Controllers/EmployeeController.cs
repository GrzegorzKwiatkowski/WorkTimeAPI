using Microsoft.AspNetCore.Mvc;
using WorkTimeManagementAPI.Models;

namespace WorkTimeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/employees")]
    
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _repository.GetEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _repository.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            await _repository.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            var existingEmployee = await _repository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                return NotFound();

            await _repository.UpdateEmployeeAsync(id, employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _repository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                return NotFound();

            await _repository.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }

}
