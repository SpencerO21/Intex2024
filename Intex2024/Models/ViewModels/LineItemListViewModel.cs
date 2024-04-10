namespace Intex2024.Models.ViewModels
{
    public class LineItemListViewModel
    {
        public IQueryable<Product>? products { get; set; }
        public IQueryable<LineItem>? lineItems { get; set; }
    }
}
