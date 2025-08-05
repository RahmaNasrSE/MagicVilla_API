using MagicVilla_Web.Models.Dtos;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> UpdateAsync<T>(VillaUpDateDto dto);
        Task<T> CreateAsync<T>(VillaCreateDto dto);
        Task<T> DeleteAsync<T>(int id);

    }
}
