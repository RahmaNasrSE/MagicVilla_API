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

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IvillaNumberRepository _dbVillaNumber;
        private readonly IvillaRepository _dbVilla;


        public IMapper _mapper { get; }

        //private readonly ILogging _logger;

        public VillaNumberAPIController(IvillaNumberRepository dbVillaNumber, IMapper mapper , IvillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();
            //_logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbersasync()
        {
            //_logger.Log("Getting all Vallias" ," ");
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
        //[ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    //_logger.Log("Get villa Error with Id" + id , "error");
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
        //[ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> CraeteVillaNumberasync([FromBody] VillaNumberCreateDto villaNumberCreateDto)
        {
            try 
            { 
            if(await _dbVillaNumber.GetAsyna(u => u.VillaNo == villaNumberCreateDto.VillaNo)!= null)
            {
                ModelState.AddModelError("Custom Error", "Villa  Already Exists!");
                return BadRequest(ModelState);
            }
            if(await _dbVilla.GetAsyna(u => u.Id == villaNumberCreateDto.VillaID) == null)
            {
                ModelState.AddModelError("custom errer", "villaId is Invaild");
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

            return CreatedAtRoute("GetVilla",new { id = villaNumber.VillaNo } , _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage =
                    new List<string> { ex.ToString() };
            }
            return _response;
        }

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
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumbersasync(int id , [FromBody] VillaNumberUdatedDto villaNumberUpDateDto)
        {
            try
            {
            if(villaNumberUpDateDto == null || id != villaNumberUpDateDto.VillaNo)
            {
                return BadRequest();
            }
            if (await _dbVilla.GetAsyna(u => u.Id == villaNumberUpDateDto.VillaID) == null)
            {
                ModelState.AddModelError("custom errer", "villaId is Invaild");
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