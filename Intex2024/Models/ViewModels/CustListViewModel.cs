namespace Intex2024.Models.ViewModels
{
    public class CustListViewModel
    {
        public IQueryable<Customer>? customers { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public int SelectedPageSize { get; set; }
    }
}
