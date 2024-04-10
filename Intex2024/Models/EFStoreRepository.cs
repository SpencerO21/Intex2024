namespace Intex2024.Models;

public class EFStoreRepository : IStoreRepository
{
    private IntexStoreContext _context;

    public EFStoreRepository(IntexStoreContext temp)
    {
        _context = temp;
    }

    public IQueryable<Product> Products => _context.Products;


    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public Product GetProductById(short id)
    {
        return _context.Products.Single(x => x.ProductId == id);
    }

    public void RemoveProduct(Product product)
    {
        _context.Remove(product);
        _context.SaveChanges();
    }

    public void AddProduct(Product product)
    {
        _context.Add(product);
        _context.SaveChanges();
    }

    public IQueryable<Transaction> Transactions => _context.Transactions;
    public IQueryable<Customer> Customers => _context.Customers;

}