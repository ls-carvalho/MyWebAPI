namespace MyWebAPI.DataTransferObject;

public class AddAddonsToProductDto
{
    public int Id { get; set; }
    public ICollection<CreateAddonWithoutProductIdDto> Addons { get; set; } = new List<CreateAddonWithoutProductIdDto>();
}
