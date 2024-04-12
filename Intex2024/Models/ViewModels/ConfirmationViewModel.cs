namespace Intex2024.Models.ViewModels;

public class ConfirmationViewModel
{
    public IQueryable<LineItem>? LineItems { get; set; }
    public Customer cust { get; set; }
    public Cart cart { get; set; }
    public IQueryable<Product> Products { get; set; }
    public Transaction Transaction { get; set; }
}