namespace MyWebAPI.DataTransferObject.User;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Models.Account Account { get; set; } = new Models.Account();
}
