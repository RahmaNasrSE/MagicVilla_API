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
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IvillaRepository _dbVilla;

        public IMapper _mapper { get; }

        

        public VillaAPIController(IvillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new();
            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillasasync()
        {
            
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsyna();
                _response.Result = _mapper.Map<List<VillaDto>>(villaList);
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
        
        [HttpGet("{id:int}" , Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsyna(u => u.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> CraeteVillasasync([FromBody] VillaCreateDto villaCreateDto)
        {
            try 
            { 
            if(await _dbVilla.GetAsyna(u => u.Name.ToLower() == villaCreateDto.Name.ToLower())!= null)
            {
                ModelState.AddModelError("ErrorMessage", "Villa Already Exists!");
                return BadRequest(ModelState);
            }
            if (villaCreateDto == null)
            {
                return BadRequest(villaCreateDto);
            }
            
            Villa villa = _mapper.Map<Villa>(villaCreateDto);

            await _dbVilla.CreateAsyna(villa);
            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla",new { id = villa.Id } , _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage =
                    new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVillasasync(int id)
        {
            try 
            { 
            if(id == 0)
            {
                return BadRequest();
            }
            var villa =  await _dbVilla.GetAsyna(u =>u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            await _dbVilla.RemoveAsyna(villa);
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
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillasasync(int id , [FromBody] VillaUpDateDto villaUpDateDto)
        {
            try
            {
            if(villaUpDateDto == null || id != villaUpDateDto.Id)
            {
                return BadRequest();
            }
            
            Villa Model = _mapper.Map<Villa>(villaUpDateDto);
            
            await _dbVilla.UpdateAsyna(Model);
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
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePartialVillasasync(int id, JsonPatchDocument<VillaUpDateDto> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsyna(u => u.Id == id , tracked:false);
            VillaUpDateDto villaDto = _mapper.Map<VillaUpDateDto>(villa);
           
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDto, ModelState);
            Villa Model = _mapper.Map<Villa>(villaDto);

            await _dbVilla.UpdateAsyna(Model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }
    }
}