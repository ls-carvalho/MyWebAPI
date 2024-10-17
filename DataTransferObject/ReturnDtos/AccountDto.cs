namespace MyWebAPI.DataTransferObject.ReturnDtos;

public class AccountDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
}