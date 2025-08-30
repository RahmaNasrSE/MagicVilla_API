using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Models.Dtos;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            
            CreateMap<MagicVilla_VillaAPI.Models.Dtos.VillaDto, MagicVilla_VillaAPI.Models.Dtos.VillaCreateDto>().ReverseMap();
            CreateMap<MagicVilla_VillaAPI.Models.Dtos.VillaDto, MagicVilla_VillaAPI.Models.Dtos.VillaUpDateDto>().ReverseMap();

            CreateMap<Models.Dtos.VillaNumberUpdateDto, MagicVilla_VillaAPI.Models.Dtos.VillaNumberCreateDto>().ReverseMap();
            CreateMap<MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto, MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto>().ReverseMap();
            CreateMap<Models.Dtos.VillaNumberUpdateDto, VillaNumber>().ReverseMap();


            CreateMap<VillaNumber, MagicVilla_VillaAPI.Models.Dtos.VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();


            CreateMap<VillaNumber, Models.Dtos.VillaNumberUpdateDto>().ReverseMap();


            CreateMap<Villa, MagicVilla_VillaAPI.Models.Dtos.VillaCreateDto>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto>().ReverseMap();

            // Dto ↔ UpdateDto
            CreateMap<VillaNumberDto, MagicVilla_VillaAPI.Models.Dtos.VillaNumberUpdateDto>().ReverseMap();
        }
    }
}
