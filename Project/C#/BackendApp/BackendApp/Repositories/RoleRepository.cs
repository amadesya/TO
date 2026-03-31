using BackendApp.AutoGenModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace BackendApp.Repositories
{
    public class RoleRepository: IRoleRepository
    {
        private static ConcurrentDictionary<int, Role>? roleCache;

        private WarehouseContext db;

        public RoleRepository(WarehouseContext context)
        {
            db = context;
            if (roleCache == null)
            {
                roleCache = new ConcurrentDictionary<int, Role>(
                    db.Roles.ToDictionary(r => r.Id));
            }
        }
        public async Task<Role?> CreateAsync(Role role)
        {
            role.Id = role.Id;
            EntityEntry<Role> added = await db.Roles.AddAsync(role);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (roleCache is null) return role;
                return roleCache.AddOrUpdate(role.Id, role, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<Role>> RetrieveAllAsync()
        {
            return Task.FromResult(roleCache is null
                ? Enumerable.Empty<Role>() : roleCache.Values);
        }

        public Task<Role?> RetrieveAsync(int id)
        {
            if (roleCache is null) return null!;
            roleCache.TryGetValue(id, out Role? role);
            return Task.FromResult(role);
        }

        private Role UpdateCache(int id, Role role)
        {
            Role? old;
            if (roleCache is not null)
            {
                if (roleCache.TryGetValue(id, out old))
                {
                    if (roleCache.TryUpdate(id, role, old))
                    {
                        if (roleCache.TryUpdate(id, role, old))
                        {
                            return role;
                        }
                    }
                }
            }
            return null!;
        }

        public async Task<Role?> UpdateAsync(int id, Role role)
        {
            db.Roles.Update(role);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return UpdateCache(id, role);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            Role? role = db.Roles.Find(id);
            if (role is null) return null;
            db.Roles.Remove(role);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (roleCache is null) return null;
                return roleCache.TryRemove(id, out role);
            }
            else
            {
                return null;
            }
        }
    }
}
