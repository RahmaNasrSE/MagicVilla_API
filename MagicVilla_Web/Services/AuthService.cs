using MagicVilla_Utility;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _ClientFactory;
        private string villaUrl;
        public AuthService(IHttpClientFactory ClientFactory, IConfiguration configuration) : base(ClientFactory)
        {
            _ClientFactory = ClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = villaUrl + "/api/v1/UserAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDto obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = villaUrl + "/api/v1/UserAuth/register"
            });
        }
    }
}
