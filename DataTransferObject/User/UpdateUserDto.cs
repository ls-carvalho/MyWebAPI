namespace MyWebAPI.DataTransferObject.User;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Models.Account Account { get; set; } = new Models.Account();
}
