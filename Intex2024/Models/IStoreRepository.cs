namespace Intex2024.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }
    
}