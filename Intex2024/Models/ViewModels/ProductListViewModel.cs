using Intex2024.Models.ViewModels;

namespace Intex2024.Models.ViewModels;

public class ProductListViewModel
{
    public IQueryable<Product>? products { get; set; }
    public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
    public int SelectedPageSize { get; set; }
    
    public string? currentCat { get; set; }
    public string? currentColor { get; set; }
    public Product Product1 { get; set; }
    public Product Product2 { get; set; }
    public Product Product3 { get; set; }
    public Customer Customer {  get; set; }
}