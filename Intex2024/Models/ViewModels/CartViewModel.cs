namespace Intex2024.Models.ViewModels;

public class CartViewModel
{
    public IQueryable<LineItem>? LineItems { get; set; }
    public Customer cust { get; set; }
    public Cart cart { get; set; }
}