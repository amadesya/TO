using BackendApp.AutoGenModels;
using BackendApp.DTO;
using BackendApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository repo;

        private readonly WarehouseContext db;

        public EmployeeController(IEmployeeRepository repo, WarehouseContext context)
        {
            this.repo = repo;
            db = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public async Task<IEnumerable<Employee>> GetEmployees(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                .Where(employee => employee.Email == email);
            }
        }

        [HttpGet("{id}", Name = nameof(GetEmployees))]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployee(int id)
        {
            Employee? employee = await repo.RetrieveAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Employee))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            Employee? addedEmployee = await repo.CreateAsync(employee);
            if (addedEmployee == null)
            {
                return BadRequest("Failed to create employee.");
            }
            else
            {
                return CreatedAtRoute(
                    routeName: nameof(GetEmployee),
                    routeValues: new
                    {
                        id = addedEmployee.Id
                    },
                    value: addedEmployee);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
            int id, [FromBody] Employee employee)
        {
            if (employee == null || employee.Id != id)
            {
                return BadRequest();
            }
            Employee? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            await repo.UpdateAsync(id, employee);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            Employee? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult();
            }
            else
            {
                return BadRequest(
                    $"Employee {id} was found but failed to delete.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == request.Email);

            if (employee == null)
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            if (employee.PasswordHash != request.Password)
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            return Ok(new
            {
                Id = employee.Id,
                FullName = employee.FullName,
                RoleId = employee.RoleId,
                message = "Успешный вход"
            });
        }
    }
}
