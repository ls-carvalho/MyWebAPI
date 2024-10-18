namespace MyWebAPI.DataTransferObject;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AccountDisplayName { get; set; } = string.Empty;
}
