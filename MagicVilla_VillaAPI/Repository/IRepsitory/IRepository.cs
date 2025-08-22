using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepsitory
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsyna(Expression<Func<T, bool>>? filter = null , string? includeProperties = null);
        Task<T> GetAsyna(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);

        Task CreateAsyna(T entity);
        Task RemoveAsyna(T entity);
        Task SaveAsyna();
    }
}
