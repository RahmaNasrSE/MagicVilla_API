using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;
using System;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Models.VM;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _ClientFactory;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory ClientFactory , IConfiguration configuration) : base(ClientFactory)
        {
            _ClientFactory = ClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/VillaNumberAPI"
            });
        }

        

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/VillaNumberAPI" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberAPI" + id
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/VillaNumberAPI" + dto.VillaNo
                //  / api / VillaAPIController /
            });
        }

       
    }
}
