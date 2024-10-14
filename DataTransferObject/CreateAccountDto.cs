using MyWebAPI.Models;

namespace MyWebAPI.DataTransferObject;

public class CreateAccountDto
{
    public string DisplayName { get; set; } = string.Empty;
    public User User { get; set; } = new User();
}
