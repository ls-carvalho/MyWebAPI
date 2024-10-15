namespace MyWebAPI.DataTransferObject.ReturnDtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public ICollection<int> AddonIds { get; set; } = new List<int>();
}
