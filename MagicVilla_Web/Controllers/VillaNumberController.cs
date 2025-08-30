using AutoMapper;

using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_Web.Models;


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

            var response = await _villaNumberService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
            if (response != null && response.IsSuccess)
            {

                list = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaNumberDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new();
            var response = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
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

                var response = await _villaNumberService.CreateAsync<MagicVilla_VillaAPI.Models.APIResponse>(dto);
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

            var resp = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
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

            // Populate VillaList تاني لو حصل Error في ModelState
            var res = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
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
        // GET: Update VillaNumber
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumberVM = new();

            // Get VillaNumber details
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
                //villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);
            }

            // Get Villa List for dropdown
            var villaResponse = await _villaService.GetAllAsync<APIResponse>();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                // تحويل من Web VM → API Dto
                var updateDto = _mapper.Map<MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto>(model.VillaNumber);

                var response = await _villaNumberService.UpdateAsync<APIResponse>(updateDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Number updated successfully!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            // Populate VillaList again لو حصل Error
            var villaResponse = await _villaService.GetAllAsync<APIResponse>();
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

        // GET: Delete VillaNumber
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            var villaResponse = await _villaService.GetAllAsync<APIResponse>();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number deleted successfully!";
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
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
        //    VillaNumberUpdateVM villaNumberVM = new();
        //    var response = await _villaNumberService.GetAsync<MagicVilla_VillaAPI.Models.APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
        //        villaNumberVM.VillaNumber = _mapper.Map<Models.Dtos.VillaNumberUpdateDto>(model);
        //    }
        //    response = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
        //    if (response != null && response.IsSuccess)
        //    {
        //        villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>
        //            (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.Name,
        //                Value = i.Id.ToString()
        //            });
        //        return View(villaNumberVM);
        //    }

        //    return RedirectToAction(nameof(IndexVillaNumber));
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        //{
        //    if (ModelState.IsValid)
        //    {


        //        var response = await _villaNumberService.UpdateAsync<MagicVilla_VillaAPI.Models.APIResponse>(model.VillaNumber);
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexVillaNumber));
        //        }
        //        else
        //        {
        //            if (response.ErrorMessage.Count > 0)
        //            {
        //                ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
        //            }
        //        }
        //    }

        //    var resp = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
        //    if (resp != null && resp.IsSuccess)
        //    {
        //        model.VillaList = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>
        //            (Convert.ToString(resp.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.Name,
        //                Value = i.Id.ToString()
        //            });
        //    }
        //    return View(model);

        //}
        // GET: Update VillaNumber
    //    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    //    {
    //        VillaNumberUpdateVM villaNumberVM = new();

    //        // Get VillaNumber details
    //        var response = await _villaNumberService.GetAsync<MagicVilla_VillaAPI.Models.APIResponse>(villaNo);
    //        if (response != null && response.IsSuccess)
    //        {
    //            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
    //            villaNumberVM.VillaNumber = _mapper.Map<MagicVilla_Web.Models.Dtos.VillaNumberUpdateDto>(model);
    //        }

    //        // Get Villa List for dropdown
    //        var villaResponse = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
    //        if (villaResponse != null && villaResponse.IsSuccess)
    //        {
    //            villaNumberVM.VillaList = JsonConvert
    //                .DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>(Convert.ToString(villaResponse.Result))
    //                .Select(i => new SelectListItem
    //                {
    //                    Text = i.Name,
    //                    Value = i.Id.ToString()
    //                });
    //        }

    //        return View(villaNumberVM);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            // تحويل من VM → Dto
    //            var updateDto = _mapper.Map<MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto>(model.VillaNumber);

    //            var response = await _villaNumberService.UpdateAsync<MagicVilla_VillaAPI.Models.APIResponse>(updateDto);
    //            if (response != null && response.IsSuccess)
    //            {
    //                TempData["success"] = "Villa Number updated successfully!";
    //                return RedirectToAction(nameof(IndexVillaNumber));
    //            }
    //        }

    //        // Populate VillaList again لو حصل Error
    //        var villaResponse = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
    //        if (villaResponse != null && villaResponse.IsSuccess)
    //        {
    //            model.VillaList = JsonConvert
    //                .DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>(Convert.ToString(villaResponse.Result))
    //                .Select(i => new SelectListItem
    //                {
    //                    Text = i.Name,
    //                    Value = i.Id.ToString()
    //                });
    //        }

    //        return View(model);
    //    }




    //    public async Task<IActionResult> DeleteVillaNumber(int villaId)
    //    {
    //        VillaNumberUpdateVM villaNumberVM = new();
    //        var response = await _villaNumberService.GetAsync<MagicVilla_VillaAPI.Models.APIResponse>(villaId);
    //        if (response != null && response.IsSuccess)
    //        {
                
    //            VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
    //            villaNumberVM.VillaNumber = _mapper.Map<Models.Dtos.VillaNumberUpdateDto>(model);
    //        }
    //        response = await _villaService.GetAllAsync<MagicVilla_VillaAPI.Models.APIResponse>();
    //        if (response != null && response.IsSuccess)
    //        {
    //            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<MagicVilla_VillaAPI.Models.Dtos.VillaDto>>
    //                (Convert.ToString(response.Result)).Select(i => new SelectListItem
    //                {
    //                    Text = i.Name,
    //                    Value = i.Id.ToString()
    //                });
    //            return View(villaNumberVM);
    //        }

    //        return RedirectToAction(nameof(IndexVillaNumber));
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
    //    {

    //        var response = await _villaNumberService.DeleteAsync<MagicVilla_VillaAPI.Models.APIResponse>(model.VillaNumber.VillaNo);
    //        if (response != null && response.IsSuccess)
    //        {
    //            return RedirectToAction(nameof(IndexVillaNumber));
    //        }


    //        return View(model);
    //    }
    //}
//}
