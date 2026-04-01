namespace BackendApp.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity);
        Task<IEnumerable<T>> RetrieveAllAsync();
        Task<T?> RetrieveAsync(int id);
        Task<T?> UpdateAsync(int id, T entity);
        Task<bool?> DeleteAsync(int id);
    }
}
