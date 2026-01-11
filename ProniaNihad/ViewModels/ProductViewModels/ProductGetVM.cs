namespace ProniaWebNihad.ViewModels.ProductViewModels
{
    public class ProductGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int RatingCount { get; set; }
        public string CategoryName { get; set; } = null!;
        public string MainImagePath { get; set; } = null!;
        public string HoverImagePath { get; set; } = null!;
        public List<string> TagNames { get; set; } = [];
        public List<string> ImagePath { get; set; } = [];

    }
}