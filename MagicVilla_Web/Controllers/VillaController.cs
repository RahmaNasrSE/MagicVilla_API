using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService , IMapper mapper) 
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        //public async Task<IActionResult> IndexVilla()
        //{
        //    List<VillaDto> list = new();

        //    var response = await _villaService.GetAllAsync<APIResponse>();
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
        //    }
        //    Console.WriteLine("Villas Count: " + list.Count);
        //    return View(list);
        //}
        
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDto> list = new();

            var response = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var jsonString = JsonConvert.SerializeObject(response.Result);
                Console.WriteLine("JSON String: " + jsonString);
                Console.WriteLine("Result Type: " + response.Result.GetType().Name);
                Console.WriteLine("Raw Result: " + response.Result);
                list = JsonConvert.DeserializeObject<List<VillaDto>>(jsonString);
            }

            Console.WriteLine("Villas Count: " + list.Count);

            return View(list);
        }
        public async Task<IActionResult> CreateVilla()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDto model)
        {
            Console.WriteLine("Model Valid: " + ModelState.IsValid);
            Console.WriteLine(JsonConvert.SerializeObject(model));
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<MagicVilla_VillaAPI.Models.APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Created Successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }
        
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<MagicVilla_VillaAPI.Models.APIResponse>(villaId);
            if (response != null && response.IsSuccess)
            {
                VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpDateDto>(model));
            }
            return RedirectToAction(nameof(IndexVilla));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpDateDto model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa Updated Successfully";
                var response = await _villaService.UpdateAsync<MagicVilla_VillaAPI.Models.APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }


        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<MagicVilla_VillaAPI.Models.APIResponse>(villaId);
            if (response != null && response.IsSuccess)
            {
                VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
                return View(model);
            }
            return RedirectToAction(nameof(IndexVilla));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDto model)
        {
            
                var response = await _villaService.DeleteAsync<MagicVilla_VillaAPI.Models.APIResponse>(model.Id);
                if (response != null && response.IsSuccess)
                {
                TempData["success"] = "Villa Deleted Successfully";
                return RedirectToAction(nameof(IndexVilla));
                }
            TempData["error"] = "Error encountered";
            return View(model);
        }

    }
}
