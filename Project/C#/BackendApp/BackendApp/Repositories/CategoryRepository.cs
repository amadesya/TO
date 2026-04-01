using BackendApp.AutoGenModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace BackendApp.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private static ConcurrentDictionary<int, Category>? categoryCache;

        private WarehouseContext db;

        public CategoryRepository(WarehouseContext context)
        {
            db = context;
            if (categoryCache == null)
            {
                categoryCache = new ConcurrentDictionary<int, Category>(
                    db.Categories.ToDictionary(c => c.Id));
            }
        }

        public async Task<Category?> CreateAsync(Category category)
        {
            EntityEntry<Category> added = await db.Categories.AddAsync(category);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                if (categoryCache is null) return category;
                return categoryCache.AddOrUpdate(category.Id, category, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<Category>> RetrieveAllAsync()
        {
            return Task.FromResult(categoryCache is null
                ? Enumerable.Empty<Category>() : categoryCache.Values);
        }

        public Task<Category?> RetrieveAsync(int id)
        {
            if (categoryCache is null) return null!;
            categoryCache.TryGetValue(id, out Category? category);
            return Task.FromResult(category);
        }

        private Category UpdateCache(int id, Category category)
        {
            Category? old;
            if (categoryCache is not null)
            {
                if (categoryCache.TryGetValue(id, out old))
                {
                    // Обновляем кэш новым объектом
                    if (categoryCache.TryUpdate(id, category, old))
                    {
                        return category;
                    }
                }
            }
            return null!;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            db.Categories.Update(category);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return UpdateCache(id, category);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Category? category = db.Categories.Find(id);
            if (category is null) return null;

            db.Categories.Remove(category);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                if (categoryCache is null) return null;
                return categoryCache.TryRemove(id, out category);
            }
            else
            {
                return null;
            }
        }
    }
}