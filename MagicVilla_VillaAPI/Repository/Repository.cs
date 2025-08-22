using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepsitory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
       
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
          
        public Repository(ApplicationDbContext db)
        {
            _db = db;
           _db.villaNumbers.Include(u => u.villa).ToList();
            this.dbSet = _db.Set<T>();
        }
        public async Task CreateAsyna(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsyna();
        }

        public async Task<T> GetAsyna(Expression<Func<T, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeprop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsyna(Expression<Func<T, bool>>? filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeprop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return await query.ToListAsync();

        }

        public async Task RemoveAsyna(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsyna();
        }

        public async Task SaveAsyna()
        {
            await _db.SaveChangesAsync();
        }

       
    }
}
