using Microsoft.CodeAnalysis;

namespace Intex2024.Models;

public class Cart

{

    private IStoreRepository _repo;
    public Cart(IStoreRepository temp) 
    { 
        _repo = temp;
    }
    public List<LineItem> Lines { get; set; } = new List<LineItem>();

    public virtual void AddItem(int transactionId, short productId, int quantity)
    {
        LineItem? line = Lines
            .Where(x => x.TransactionId == transactionId)
            .FirstOrDefault();

        Product product = _repo.Products.FirstOrDefault(x => x.ProductId == productId);

        //Has this item already been added to our cart?
        if (line == null)
        {
            Lines.Add(new LineItem()
            {
                ProductId = product.ProductId,
                Qty = quantity

            });
        }
        else
        {
            line.Qty += quantity;
        }
    }

    public virtual void RemoveLine(Product prod) => Lines.RemoveAll(x => x.ProductId == prod.ProductId);

    public virtual void Clear() => Lines.Clear();

    public decimal CalculateTotal()
    {
        decimal total = 0;
        foreach (LineItem line in Lines)
        {
            Product product = _repo.Products.FirstOrDefault(x => x.ProductId == line.ProductId);
            total += (product.Price * line.Qty);
        }
        return total;
    }


}
