namespace MyWebAPI.DataTransferObject;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public CreateAccountDto Account { get; set; } = new CreateAccountDto();
}
