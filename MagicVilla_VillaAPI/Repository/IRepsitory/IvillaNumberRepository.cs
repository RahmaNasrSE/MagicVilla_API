using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepsitory
{
    public interface IvillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsyna(VillaNumber entity);
       
    }
}
