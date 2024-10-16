namespace MyWebAPI.DataTransferObject;

public class AddAddonsToProductDto
{
    public int Id { get; set; }
    public ICollection<CreateAddonDto> Addons { get; set; } = new List<CreateAddonDto>();
}
