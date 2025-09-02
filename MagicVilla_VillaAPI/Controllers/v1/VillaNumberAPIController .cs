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

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    
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
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "string1", "string2" };
        }

        //[MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbersasync()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsyna(includeProperties:"villa");
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = 
                    new List<string> { ex.ToString() };
            }
            return _response;
        }

        

        [HttpGet("{id:int}" , Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _dbVillaNumber.GetAsyna(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage =
                    new List<string> { ex.ToString() };
            }
            return _response;

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CraeteVillaNumberasync([FromBody] VillaNumberCreateDto villaNumberCreateDto)
        {
            try 
            { 
            if(await _dbVillaNumber.GetAsyna(u => u.VillaNo == villaNumberCreateDto.VillaNo)!= null)
            {
                ModelState.AddModelError("ErrorMessage", "Villa Number Already Exist!");
                return BadRequest(ModelState);
            }
            if(await _dbVilla.GetAsyna(u => u.Id == villaNumberCreateDto.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessage", "villaId is Invaild");
                return BadRequest(ModelState);
            }
            if (villaNumberCreateDto == null)
            {
                return BadRequest(villaNumberCreateDto);
            }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDto);

            await _dbVillaNumber.CreateAsyna(villaNumber);
            _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVillaNumber",new { id = villaNumber.VillaNo } , _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage =
                    new List<string> { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumbersasync(int id)
        {
            try 
            { 
            if(id == 0)
            {
                return BadRequest();
            }
            var villaNumber =  await _dbVillaNumber.GetAsyna(u =>u.VillaNo == id);
            if(villaNumber == null)
            {
                return NotFound();
            }
            await _dbVillaNumber.RemoveAsyna(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage =
                    new List<string> { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumbersasync(int id , [FromBody] VillaNumberUpdateDto villaNumberUpDateDto)
        {
            try
            {
            if(villaNumberUpDateDto == null || id != villaNumberUpDateDto.VillaNo)
            {
                return BadRequest();
            }
            if (await _dbVilla.GetAsyna(u => u.Id == villaNumberUpDateDto.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessage", "villaId is Invaild");
                return BadRequest(ModelState);
            }

                VillaNumber Model = _mapper.Map<VillaNumber>(villaNumberUpDateDto);
            
            await _dbVillaNumber.UpdateAsyna(Model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage =
                    new List<string> { ex.ToString() };
            }
            return _response;
        }
        
        
    }
}