using AutoMapper;

using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_Web.Models;
using Microsoft.AspNetCore.Authorization;
using MagicVilla_Utility;


namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<MagicVilla_VillaAPI.Models.Dtos.VillaNumberDto> list = new();

            var response = await _villaNumberService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {

                list = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaNumberDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new();
            var response = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
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
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<MagicVilla_VillaAPI.Models.Dtos.VillaNumberCreateDto>(model.VillaNumber);

                var response = await _villaNumberService.CreateAsync<MagicVilla_VillaAPI.Models.APIResponse>(dto, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessage.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
                    }
                }
            }

            var resp = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);

            
            var res = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumberVM = new();

            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            
            var villaResponse = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (villaResponse != null && villaResponse.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert
                    .DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>(Convert.ToString(villaResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var updateDto = _mapper.Map<MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto>(model.VillaNumber);

                var response = await _villaNumberService.UpdateAsync<APIResponse>(updateDto, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Number updated successfully!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            var villaResponse = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (villaResponse != null && villaResponse.IsSuccess)
            {
                model.VillaList = JsonConvert
                    .DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>(Convert.ToString(villaResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            var villaResponse = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (villaResponse != null && villaResponse.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert
                    .DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>(Convert.ToString(villaResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                return View(villaNumberVM);
            }

            return RedirectToAction(nameof(IndexVillaNumber));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo , HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number deleted successfully!";
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
}
