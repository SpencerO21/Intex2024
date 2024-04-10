namespace Intex2024.Models;

public class EFStoreRepository : IStoreRepository
{
    private IntexStoreContext _context;

    public EFStoreRepository(IntexStoreContext temp)
    {
        _context = temp;
    }

    public IQueryable<Product> Products => _context.Products;
    public IQueryable<Transaction> Transactions => _context.Transactions;
}