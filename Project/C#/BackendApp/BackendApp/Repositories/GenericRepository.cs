using BackendApp.AutoGenModels;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly WarehouseContext _context;
        private readonly DbSet<T> _dbSet; 

        public GenericRepository(WarehouseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            int affected = await _context.SaveChangesAsync();

            return affected > 0 ? entity : null;
        }

        public async Task<IEnumerable<T>> RetrieveAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> RetrieveAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> UpdateAsync(int id, T entity)
        {
            _dbSet.Update(entity);
            int affected = await _context.SaveChangesAsync();

            return affected > 0 ? entity : null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            T? entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbSet.Remove(entity);
            int affected = await _context.SaveChangesAsync();

            return affected > 0;
        }
    }
}