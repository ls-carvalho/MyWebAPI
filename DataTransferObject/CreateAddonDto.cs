namespace MyWebAPI.DataTransferObject;

public class CreateAddonDto
{
    public string Name { get; set; } = string.Empty;
    public int ProductId { get; set; }
}
