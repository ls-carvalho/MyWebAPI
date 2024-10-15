using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.DataTransferObject;

public class CreateAccountDto
{
    public string DisplayName { get; set; } = string.Empty;
    public UserDto User { get; set; } = new UserDto();
}
