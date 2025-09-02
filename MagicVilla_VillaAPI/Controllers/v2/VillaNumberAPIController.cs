using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using MagicVilla_VillaAPI.Repository.IRepsitory;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    
    //[ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IvillaNumberRepository _dbVillaNumber;
        private readonly IvillaRepository _dbVilla;


        public IMapper _mapper { get; }


        public VillaNumberAPIController(IvillaNumberRepository dbVillaNumber, IMapper mapper , IvillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new();
        }
        

        [MapToApiVersion("2.0")]
        [HttpGet("GetString")]
        public IEnumerable<string> Get() 
        {
            return new string[] { "Bhrugen", "DotNetMastery" };
        }

       
        
        
    }
}