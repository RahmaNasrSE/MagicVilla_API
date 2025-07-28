using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new List<VillaDto>
            {
                new VillaDto{Id = 1, Name ="Pool View" , Sqft = 100 ,accupancy = 4 },
                new VillaDto{Id = 2, Name ="Beach View" , Sqft = 300 ,accupancy = 3 }
            };
    } 
}
