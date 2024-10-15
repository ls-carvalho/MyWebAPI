using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.DataTransferObject;

public class AddProductAddonsDto
{
    public int Id { get; set; }
    public ICollection<AddonDto> Addons { get; set; } = new List<AddonDto>();
}
