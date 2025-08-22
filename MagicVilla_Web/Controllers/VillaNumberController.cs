using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper , IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<MagicVilla_VillaAPI.Models.Dtos.VillaNumberDto> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess )
            {
                
                list = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaNumberDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<MagicVilla_VillaAPI.Models.Dtos.VillaNumberCreateDto>(model.VillaNumber);

                var response = await _villaNumberService.CreateAsync<APIResponse>(dto);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            // Populate VillaList تاني لو حصل Error في ModelState
            var res = await _villaService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexVillaNumber));
        //        }
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> UpdateVillaNumber(int villaId)
        //{
        //    var response = await _villaService.GetAsync<APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
        //        return View(_mapper.Map<VillaNumberUdatedDto>(model));
        //    }
        //    return RedirectToAction(nameof(IndexVillaNumber));
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVillaNumber(VillaNumberUdatedDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaService.UpdateAsync<APIResponse>(model);
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexVillaNumber));
        //        }
        //    }

        //    return View(model);
        //}


        //public async Task<IActionResult> DeleteVillaNumber(int villaId)
        //{
        //    var response = await _villaService.GetAsync<APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return RedirectToAction(nameof(IndexVillaNumber));
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVillaNumber(VillaDto model)
        //{

        //    var response = await _villaService.DeleteAsync<APIResponse>(model.Id);
        //    if (response != null && response.IsSuccess)
        //    {
        //        return RedirectToAction(nameof(IndexVillaNumber));
        //    }


        //    return View(model);
        //}
    }
}
