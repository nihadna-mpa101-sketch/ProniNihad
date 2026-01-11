using ProniaWebNihad.Models;
using ProniaWebNihad.Models.Common;
    public class Tag : BaseEntity
{
    public string Name { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; } = [];
}
