namespace MyWebAPI.DataTransferObject.ReturnDtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public AccountDto Account { get; set; } = new AccountDto();

}
