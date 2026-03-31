using BackendApp.AutoGenModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace BackendApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static ConcurrentDictionary<int, Employee>? employeeCache;

        private WarehouseContext db;

        public EmployeeRepository(WarehouseContext context)
        {
            db = context;
            if (employeeCache == null)
            {
                employeeCache = new ConcurrentDictionary<int, Employee>(
                    db.Employees.ToDictionary(e => e.Id));
            }
        }
        public async Task<Employee?> CreateAsync(Employee employee)
        {
            employee.Id = employee.Id;
            EntityEntry<Employee> added = await db.Employees.AddAsync(employee);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (employeeCache is null) return employee;
                return employeeCache.AddOrUpdate(employee.Id, employee, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<Employee>> RetrieveAllAsync()
        {
            return Task.FromResult(employeeCache is null
                ? Enumerable.Empty<Employee>() : employeeCache.Values);
        }

        public Task<Employee?> RetrieveAsync(int id)
        {
            if (employeeCache is null) return null!;
            employeeCache.TryGetValue(id, out Employee? employee);
            return Task.FromResult(employee);
        }

        private Employee UpdateCache(int id, Employee employee)
        {
            Employee? old;
            if (employeeCache is not null)
            {
                if (employeeCache.TryGetValue(id, out old))
                {
                    if (employeeCache.TryUpdate(id, employee, old))
                    {
                        if (employeeCache.TryUpdate(id, employee, old))
                        {
                            return employee;
                        }
                    }
                }
            }
            return null!;
        }

        public async Task<Employee?> UpdateAsync(int id, Employee employee)
        {
            db.Employees.Update(employee);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return UpdateCache(id, employee);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Employee? employee = db.Employees.Find(id);
            if (employee is null) return null;
            db.Employees.Remove(employee);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (employeeCache is null) return null;
                return employeeCache.TryRemove(id, out employee);
            }
            else
            {
                return null;
            }
        }
    }
}
