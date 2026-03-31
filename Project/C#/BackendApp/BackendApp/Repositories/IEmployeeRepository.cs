using BackendApp.AutoGenModels;

namespace BackendApp.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> CreateAsync(Employee employee);
        Task <IEnumerable<Employee>> RetrieveAllAsync();
        Task<Employee?> RetrieveAsync(int id);
        Task<Employee?> UpdateAsync(int id, Employee employee);
        Task<bool?> DeleteAsync(int id);
    }
}
