using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.DataTransferObject;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public ICollection<AddonDto> Addons { get; set; } = new List<AddonDto>();
}
