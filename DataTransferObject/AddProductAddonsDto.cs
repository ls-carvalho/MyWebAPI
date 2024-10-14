using MyWebAPI.Models;

namespace MyWebAPI.DataTransferObject;

public class AddProductAddonsDto
{
    public int Id { get; set; }
    public ICollection<Addon> Addons { get; set; } = new List<Addon>();
}
