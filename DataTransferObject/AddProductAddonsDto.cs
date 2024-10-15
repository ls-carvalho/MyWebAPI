namespace MyWebAPI.DataTransferObject;

public class AddProductAddonsDto
{
    public int Id { get; set; }
    /* TO-DO: talvez refatorar para ICollection<CreateAddonDto> Addons */
    public ICollection<int> AddonIds { get; set; } = new List<int>();
}
