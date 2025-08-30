using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagicVilla_VillaAPI.Models.Dtos;
namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberDeleteVM
    {
        public VillaNumber VillaNumber { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
    //public class VillaNumberCreateVM
    //{

    //    public  VillaNumberCreateVM()
    //    {
    //        VillaNumber = new VillaNumberCreateVM();
    //    }
    //    public VillaNumberCreateVM VillaNumber { get; set; }
    //    [ValidateNever]
    //    public IEnumerable<SelectListItem> VillaList { get; set; }
    //}
}
