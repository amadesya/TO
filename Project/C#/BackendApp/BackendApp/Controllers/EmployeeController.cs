using Microsoft.AspNetCore.Mvc;
using BackendApp.AutoGenModels;
using BackendApp.Repositories;

namespace BackendApp.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository repo;

        public EmployeeController(IEmployeeRepository repo)
        {
            this.repo = repo;
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
    }
}
