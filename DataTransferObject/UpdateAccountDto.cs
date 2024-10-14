using MyWebAPI.Models;

namespace MyWebAPI.DataTransferObject;

public class UpdateAccountDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public User User { get; set; } = new User();
}
