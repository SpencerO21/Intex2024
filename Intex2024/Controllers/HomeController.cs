using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex2024.Models;
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

    public IActionResult Index(int pageSize = 5, int pageNum = 1)
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