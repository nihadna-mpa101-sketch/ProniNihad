using ProniaWebNihad.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ProniaWebNihad.Models
{
    public class Shipping : BaseEntity
    {
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public string ImagePath {  get; set; } = null!;

    }
}
