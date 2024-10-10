namespace MyWebAPI.DataTransferObject.Account;

public class UpdateAccountDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public Models.User User { get; set; } = new Models.User();
}
