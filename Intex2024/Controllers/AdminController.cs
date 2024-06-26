﻿using Intex2024.Models;
using Intex2024.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intex2024.Controllers
{
    [Authorize(Roles = "Admin")]
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

        //public IActionResult ViewCust(int pageSize = 10, int pageNum = 1)
        //{

        //    var custListViewModel = new CustListViewModel()
        //    {
        //        PaginationInfo = new PaginationInfo
        //        {
        //            CurrentPage = pageNum,
        //            ItemsPerPage = pageSize,
        //            TotalItems = _repo.Products.Count()
        //        },
        //        SelectedPageSize = pageSize,
        //        customers = _repo.Customers
        //            .OrderBy(x => x.LastName)
        //            .Skip((pageNum - 1) * pageSize)
        //            .Take(pageSize),
        //    };

        //    return View(custListViewModel);
        //}


        [HttpGet]
        public IActionResult EditProduct(short id)
        {
            var recordToEdit = _repo.GetProductById(id);
            return View(recordToEdit);
        }

        [HttpPost]
        public IActionResult EditProduct(Product updatedProduct)
        {
            _repo.UpdateProduct(updatedProduct);
            return RedirectToAction("ViewProducts");
        }

        [HttpGet]
        public IActionResult EditCustomer(short id)
        {
            var recordToEdit = _repo.GetCustomerById(id);
            return View(recordToEdit);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer updatedCust)
        {
            _repo.UpdateCustomer(updatedCust);
            return RedirectToAction("ViewCust");
        }

        [HttpGet]
        public IActionResult DeleteProduct(short id)
        {
            var recordToDelete = _repo.Products.Single(x => x.ProductId == id);
            _repo.RemoveProduct(recordToDelete);
            return RedirectToAction("ViewProducts");
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            _repo.RemoveProduct(product);
            return RedirectToAction("ViewProducts");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View("EditProduct", new Product());
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var lastProduct = _repo.Products.OrderByDescending(t => t.ProductId).FirstOrDefault();
                product.ProductId = (short)(lastProduct.ProductId + 1);
                _repo.AddProduct(product);
                return RedirectToAction("ViewProducts");
            }
            else
            {
                return View("EditProduct", product);
            }
        }

        public IActionResult ViewOrders(int pageSize = 200, int pageNum = 1)
        {

            var orderListViewModel = new OrderListViewModel()
            {
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Transactions.Count()
                },
                SelectedPageSize = pageSize,
                transactions = _repo.Transactions
                    .OrderByDescending(x => x.Date)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
            };

            return View(orderListViewModel);
        }

        public IActionResult ViewFraudOrders(int pageSize = 200, int pageNum = 1)
        {
            var orderListViewModel = new OrderListViewModel()
            {
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Transactions.Count(x => x.Fraud)
                },
                SelectedPageSize = pageSize,
                transactions = _repo.Transactions
                    .Where(x => x.Fraud)
                    .OrderByDescending(x => x.Date)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
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

        public IActionResult AdminPortal()
        {
             return View();
        }

        public IActionResult ViewOrderDetails(int id)
        {
            // Query for the product and each related item
            var transaction = _repo.Transactions.FirstOrDefault(x => x.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

    }
}
