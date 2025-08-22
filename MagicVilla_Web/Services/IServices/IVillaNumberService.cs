using MagicVilla_VillaAPI.Models.Dtos;

public interface IVillaNumberService
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> UpdateAsync<T>(VillaNumberUdatedDto dto);
    Task<T> CreateAsync<T>(VillaNumberCreateDto dto);
    Task<T> DeleteAsync<T>(int id);
}

//using MagicVilla_VillaAPI.Models;
//using MagicVilla_Web.Models.Dtos;
//using MagicVilla_Web.Models.VM;

//namespace MagicVilla_Web.Services.IServices
//{
//    public interface IVillaNumberService
//    {
//        Task<T> GetAllAsync<T>();
//        Task<T> GetAsync<T>(int id);
//        Task<T> UpdateAsync<T>(VillaNumberUdatedDto dto);
//        Task<T> CreateAsync<T>(VillaNumberCreateDto dto);
//        Task<T> DeleteAsync<T>(int id);
//        Task<T> CreateAsync<T>(VillaNumberCreateVM villaNumber);

//    }
//}
