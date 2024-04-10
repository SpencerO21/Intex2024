namespace Intex2024.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }
    public void UpdateProduct(Product product);
    public void RemoveProduct(Product product);
}