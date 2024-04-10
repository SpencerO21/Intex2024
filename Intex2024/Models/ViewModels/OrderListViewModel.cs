
namespace Intex2024.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IQueryable<Transaction>? transactions { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public int SelectedPageSize { get; set; }
    }
}
