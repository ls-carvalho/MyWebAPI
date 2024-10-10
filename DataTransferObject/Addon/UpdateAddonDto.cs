namespace MyWebAPI.DataTransferObject.Addon;

public class UpdateAddonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Models.Product Product { get; set; } = new Models.Product();
}
