using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepsitory;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository :  Repository<Villa> ,  IvillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<Villa> UpdateAsyna(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
