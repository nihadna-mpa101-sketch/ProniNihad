using ProniaWebNihad.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ProniaWebNihad.Models
{
 
        public class ProductImage : BaseEntity
        {
            public int ProductId { get; set; }
            public Product? Product { get; set; }
            [Required]
            [MaxLength(100)]
            public string ImagePath { get; set; } = null!;
        }
}
