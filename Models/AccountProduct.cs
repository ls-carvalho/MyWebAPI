namespace MyWebAPI.Models;

public class AccountProduct
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = new Account();
    public int ProductId { get; set; }
    public Product Product { get; set; } = new Product();
}
