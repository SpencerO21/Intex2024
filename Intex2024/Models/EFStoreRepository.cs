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
        _context.Update(product);
        _context.SaveChanges();
    }

    public void RemoveProduct(Product product)
    {
        _context.Remove(product);
        _context.SaveChanges();
    }
}