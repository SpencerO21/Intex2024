using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;
using Intex2024.Models;

namespace Intex2024.Components;

public class ProductFilterViewComponent : ViewComponent
{
    private IStoreRepository _repo;
    public ProductFilterViewComponent(IStoreRepository temp)
    {
        _repo = temp;
    }
    
    public IViewComponentResult Invoke()
    {
        

        var categories = _repo.Products
            .ToList() // This executes the query and brings the results into memory
            .SelectMany(x => new[] { x.Category1, x.Category2, x.Category3 })
            .Distinct()
            .Where(cat => !string.IsNullOrEmpty(cat))
            .OrderBy(x => x)
            .ToList()
            .AsQueryable(); // Convert the List<string?> to IQueryable<string?>

        return View(categories);
    }



}