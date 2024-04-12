using Microsoft.EntityFrameworkCore;

namespace Intex2024.Models;

public class EFStoreRepository : IStoreRepository
{
    private IntexStoreContext _context;

    public EFStoreRepository(IntexStoreContext temp)
    {
        _context = temp;
    }

    public IQueryable<Product> Products => _context.Products;


    public void UpdateTransaction(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }
    public void UpdateCustomer(Customer cust)
    {
        _context.Customers.Update(cust);
        _context.SaveChanges();
    }

    public void UpdateItemNoSave(LineItem lineItem)
    {
        _context.LineItems.Update(lineItem);
    }

    public Product GetProductById(short id)
    {
        return _context.Products.Single(x => x.ProductId == id);
    }
    public Customer GetCustomerById(short id)
    {
        return _context.Customers.Single(x => x.CustomerId == id);
    }

    public void RemoveProduct(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void AddItem(LineItem lineItem)
    {
        _context.LineItems.Add(lineItem);
        _context.SaveChanges();
    }

    public void AddTransaction(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        _context.SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Add async versions of other CRUD operations as needed
    public async Task AddTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LineItem>> GetLineItemsByCartIdAsync(int cartId)
    {
        return await _context.LineItems.Where(x => x.CartId == cartId).ToListAsync();
    }
    public void UpdateItem(LineItem lineItem)
    {
        _context.LineItems.Update(lineItem);
        _context.SaveChanges();
    }

    public void UpdateCart(Cart cart)
    {
        _context.Carts.Update(cart);
        _context.SaveChanges();
    }

    public void AddCustomer(Customer customer)
    {
        _context.Add(customer);
        _context.SaveChanges();
    }

    public IQueryable<Transaction> Transactions => _context.Transactions;
    public IQueryable<Customer> Customers => _context.Customers;
    public IQueryable<LineItem> LineItems => _context.LineItems;
    public IQueryable<Cart> Carts => _context.Carts;

}