using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.logging;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Log("Getting all Vallias" ," ");
            return Ok(VillaStore.VillaList);

        }
        [HttpGet("{id:int}" , Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDto> GetVillas(int id)
        {
            if (id == 0)
            {
                _logger.Log("Get villa Error with Id" + id , "error");
                return BadRequest();
            }
            var Villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (Villa == null)
            {
                return NotFound();
            }
            return Ok(Villa);

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDto> CraeteVillas([FromBody] VillaDto villaDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if(VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower())!= null)
            {
                ModelState.AddModelError("Custom Error", "Villa Already Exists!");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1 ;
            VillaStore.VillaList.Add(villaDto);
            return CreatedAtRoute("GetVilla",new { id = villaDto.Id } , villaDto);
        }
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteVillas(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u =>u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            VillaStore.VillaList.Remove(villa);
            return NoContent();
        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateVillas(int id , [FromBody] VillaDto villaDto)
        {
            if(villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDto.Name;
            villa.Sqft = villaDto.Sqft;
            villa.accupancy = villaDto.accupancy;
            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdatePartialVillas(int id, JsonPatchDocument<VillaDto> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }
    }
}