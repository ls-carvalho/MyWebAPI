namespace MyWebAPI.DataTransferObject.Account;

public class CreateAccountDto
{
    public string DisplayName { get; set; } = string.Empty;
    public Models.User User { get; set; } = new Models.User();
}
