using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class Addon
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int ProductId { get; set; }
    public Product Product { get; set; } = new Product();
}
