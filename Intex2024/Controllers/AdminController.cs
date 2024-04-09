﻿using Intex2024.Models;
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
    }
}