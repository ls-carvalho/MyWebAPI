namespace MyWebAPI.DataTransferObject;

public class UpdateAddonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductId { get; set; }
}
