using Intex2024.Models;
using Intex2024.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Intex2024.Controllers
{
    public class AdminController : Controller
    {
        private IStoreRepository _repo;
        public AdminController(IStoreRepository temp)
        {
            _repo = temp;
        }


        public IActionResult ViewProducts(int pageSize = 10, int pageNum = 1)
        {

            var productsListViewModel = new ProductListViewModel()
            {
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Products.Count()
                },
                SelectedPageSize = pageSize,
                products = _repo.Products
                    .OrderBy(x => x.Name)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
            };

            return View(productsListViewModel);
        }

        public IActionResult ViewOrders(bool? fraud, int pageSize = 200, int pageNum = 1)
        {
            var query = _repo.Transactions.AsQueryable();
            query = query.Where(x => x.Fraud == fraud);
            ViewBag.IsFraud = fraud;

            var transactions = query.Where(x => x.Fraud == fraud)
                .OrderByDescending(x => x.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);

            var totalItems = query.Count();
            var orderListViewModel = new OrderListViewModel()
            {
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                },
                SelectedPageSize = pageSize,
                transactions = transactions
                    
            };

            return View(orderListViewModel);
        }
        public IActionResult ViewCust(int pageSize = 100, int pageNum = 1)
        {

            var custListViewModel = new CustListViewModel()
            {
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Customers.Count()
                },
                SelectedPageSize = pageSize,
                customers = _repo.Customers
                    .OrderBy(x => x.LastName)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
            };

            return View(custListViewModel);
        }
    }
}
