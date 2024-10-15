namespace MyWebAPI.DataTransferObject;

public class AddProductAddonsDto
{
    public int Id { get; set; }
    public ICollection<int> AddonIds { get; set; } = new List<int>();
}
