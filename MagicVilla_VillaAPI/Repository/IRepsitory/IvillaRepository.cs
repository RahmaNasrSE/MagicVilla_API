using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepsitory
{
    public interface IvillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsyna(Villa entity);
       
    }
}
