using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.DataTransferObject;

public class UpdateAccountDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public UserDto User { get; set; } = new UserDto();
}
