using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dtos
{
    public class VillaUpDateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public Double Rate { get; set; }
        [Required]
        public int Sqft { get; set; }
        [Required]
        public int Accupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
