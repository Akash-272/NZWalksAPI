using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NZWalks.API.Model.Domain.DTO
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100,ErrorMessage ="Name should be maximum of 100 characters")]
        public string Name {  get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public string LengthInKm { get; set; }

        public string? WalkImageUrl {  get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }


    }
}
