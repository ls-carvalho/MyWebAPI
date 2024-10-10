namespace MyWebAPI.DataTransferObject.Addon;

public class CreateAddonDto
{
    public string Name { get; set; } = string.Empty;
    public Models.Product Product { get; set; } = new Models.Product();
}
