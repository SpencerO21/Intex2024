using Mission11.Models.ViewModels;

namespace Intex2024.Models.ViewModels;

public class ProductListViewModel
{
    public IQueryable<Product>? products { get; set; }
    public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
    public int SelectedPageSize { get; set; }
}