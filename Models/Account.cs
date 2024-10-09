using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class Account
{
    [Key]
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;

    public User User { get; set; } = new User();
    public ICollection<AccountProduct> Products { get; set; } = new List<AccountProduct>();
}
