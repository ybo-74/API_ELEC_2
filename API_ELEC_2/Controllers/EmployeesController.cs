using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Models;
using API_ELEC_2.Repositories;

namespace API_ELEC_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeesController(IConfiguration configuration)
        {
            _employeeRepository = new EmployeeRepository(configuration);
        }

        // api get employee
        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = _employeeRepository.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // get api employee id
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeId(int id)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeID(id);

                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // post api employee
        [HttpPost]
        public ActionResult CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest("Employee data is required.");
                }

                bool isCreated = _employeeRepository.CreateEmployee(employee);

                if (isCreated)
                {
                    return CreatedAtAction(nameof(GetEmployeeId),
                        new { id = employee.employeeID }, employee);
                }

                return BadRequest("Failed to create employee.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // put api employee id
        [HttpPut("{id}")]
        public ActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest("Employee data is required.");
                }

                var existingEmployee = _employeeRepository.GetEmployeeID(id);
                if (existingEmployee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }

                bool isUpdated = _employeeRepository.UpdateEmployee(id, employee);

                if (isUpdated)
                {
                    return Ok("Employee updated successfully.");
                }

                return BadRequest("Failed to update employee.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // delete
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                var existingEmployee = _employeeRepository.GetEmployeeID(id);
                if (existingEmployee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }

                bool isDeleted = _employeeRepository.DeleteEmployee(id);

                if (isDeleted)
                {
                    return Ok("Employee deleted successfully.");
                }

                return BadRequest("Failed to delete employee.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}