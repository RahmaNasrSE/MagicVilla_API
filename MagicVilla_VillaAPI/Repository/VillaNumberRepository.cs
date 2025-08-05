using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepsitory;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository :  Repository<VillaNumber> , IvillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsyna(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.villaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
