using Microsoft.EntityFrameworkCore;
using ProniaWebNihad.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ProniaWebNihad.Models
{
    public class Product: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Precision(18, 2)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public int RatingCount { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? MainImagePath { get; set; }
        public string? HoverImagePath { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = [];
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }
}
