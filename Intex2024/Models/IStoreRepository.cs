namespace Intex2024.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }

    public void UpdateProduct(Product product);
    public void RemoveProduct(Product product);
    public void AddProduct(Product product);
    public void AddItem(LineItem lineItem);

    public Product GetProductById(short id);


    public IQueryable<Transaction> Transactions { get; }
    public IQueryable<Customer> Customers { get; }

}