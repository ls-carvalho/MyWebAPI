using MyWebAPI.Models;

namespace MyWebAPI.DataTransferObject;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public ICollection<Addon> Addons { get; set; } = new List<Addon>();
}
