using ProniaWebNihad.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ProniaWebNihad.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; } = null!;
    }
}
