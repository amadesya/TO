using BackendApp.AutoGenModels;

namespace BackendApp.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> CreateAsync(Role role);
        Task<IEnumerable<Role>> RetrieveAllAsync();
        Task<Role?> RetrieveAsync(int id);
        Task<Role?> UpdateAsync(int id, Role role);
        Task<bool?> DeleteAsync(int id);
    }
}
