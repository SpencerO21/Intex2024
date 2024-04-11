namespace Intex2024.Models;

public interface IStoreRepository
{
    public IQueryable<Product> Products { get; }

    public void UpdateProduct(Product product);
    public void RemoveProduct(Product product);
    public void AddProduct(Product product);

    public Product GetProductById(short id);


    public IQueryable<Transaction> Transactions { get; }
    public void UpdateOrder(Transaction order);
    public Transaction GetTransactionById(int id);
    public IQueryable<Customer> Customers { get; }

}