namespace Intex2024.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }
    public void UpdateTransaction(Transaction transaction);
    public void UpdateProduct(Product product);
    public void RemoveProduct(Product product);
    public void AddProduct(Product product);
    public void AddItem(LineItem lineItem);
    public void UpdateItem(LineItem lineItem);
    public void UpdateCart(Cart cart);
    public void UpdateItemNoSave(LineItem lineItem);
    public void SaveChanges();
    public Product GetProductById(short id);

    public void AddTransaction(Transaction transaction);
    public IQueryable<Transaction> Transactions { get; }
    public IQueryable<Customer> Customers { get; }
    public IQueryable<LineItem> LineItems { get; }
    public IQueryable<Cart> Carts { get; }
    
    Task<int> SaveChangesAsync();
    Task AddTransactionAsync(Transaction transaction);
    Task<List<LineItem>> GetLineItemsByCartIdAsync(int cartId);

    

}