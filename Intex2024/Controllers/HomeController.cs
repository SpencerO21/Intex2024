using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex2024.Models;
using Intex2024.Models.ViewModels;
using Intex2024.Models.ViewModels;

namespace Intex2024.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IStoreRepository _repo;
    public HomeController(ILogger<HomeController> logger, IStoreRepository temp)
    {
        _logger = logger;
        _repo = temp;
    }

    public IActionResult Index(string? cat, string? color, int pageSize = 5, int pageNum = 1)
    {
        var query = _repo.Products.AsQueryable();
        
        if (!string.IsNullOrEmpty(cat))
        {
            query = query.Where(x => x.Category1 == cat || x.Category2 == cat || x.Category3 == cat);
            ViewBag.SelectedCategory = cat;
        }

        if (!string.IsNullOrEmpty(color))
        {
            query = query.Where(x => x.PrimaryColor == color || x.SecondaryColor == color);
            ViewBag.SelectedColors = color;
        }

        var totalItems = query.Count();

        var products = query
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);

        var viewModel = new ProductListViewModel
        {
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            },
            currentCat = cat,
            currentColor = color,
            SelectedPageSize = pageSize,
            products = products
        };

        return View(viewModel);
    }


    
    public ActionResult ProductDetails(int id)
    {
        var product = _repo.Products.FirstOrDefault(x => x.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}